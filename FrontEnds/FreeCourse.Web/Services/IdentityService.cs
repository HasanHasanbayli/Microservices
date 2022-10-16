using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FreeCourse.Web.Services;

public class IdentityService : IIdentityService
{
    private readonly ClientSettings _clientSettings;
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ServiceApiSettings _serviceApiSettings;

    public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
        IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _clientSettings = clientSettings.Value;
        _serviceApiSettings = serviceApiSettings.Value;
    }

    public async Task<Response<bool>> SignIn(LoginRequest loginRequest)
    {
        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceApiSettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError) throw disco.Exception;

        var passwordTokenRequest = new PasswordTokenRequest
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            UserName = loginRequest.Email,
            Password = loginRequest.Password,
            Address = disco.TokenEndpoint
        };

        var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

        if (token.IsError)
        {
            var responseContent = await token.HttpResponse.Content.ReadAsStreamAsync();

            var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Response<bool>.Fail(errorDto!.Errors, 400);
        }

        var userInfoRequest = new UserInfoRequest
        {
            Token = token.AccessToken,
            Address = disco.UserInfoEndpoint
        };

        var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

        if (userInfo.IsError) throw userInfo.Exception;

        var claimsIdentity = new ClaimsIdentity(userInfo.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationProperties = new AuthenticationProperties();

        authenticationProperties.StoreTokens(new List<AuthenticationToken>
        {
            new() { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
            new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
            new()
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
            }
        });

        authenticationProperties.IsPersistent = loginRequest.IsRemember;

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, authenticationProperties);

        return Response<bool>.Success(200);
    }

    public async Task<TokenResponse> GetAccessTokenByRefreshToken()
    {
        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceApiSettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError) throw disco.Exception;

        var refreshToken =
            await _httpContextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        RefreshTokenRequest refreshTokenRequest = new()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            RefreshToken = refreshToken,
            Address = disco.TokenEndpoint
        };

        var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

        if (token.IsError) return null!;

        var authenticationTokens = new List<AuthenticationToken>
        {
            new() { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
            new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
            new()
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
            }
        };

        var authenticationResult = await _httpContextAccessor.HttpContext!.AuthenticateAsync();

        var properties = authenticationResult.Properties;

        properties!.StoreTokens(authenticationTokens);

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            authenticationResult.Principal!, properties);

        return token;
    }

    public async Task RevokeRefreshToken()
    {
        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceApiSettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError) throw disco.Exception;

        var refreshToken =
            await _httpContextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        TokenRevocationRequest tokenRevocationRequest = new()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            Address = disco.RevocationEndpoint,
            Token = refreshToken,
            TokenTypeHint = "refresh_token"
        };

        await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
    }
}