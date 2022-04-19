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
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClientSettings _clientSettings;
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
            Address = _serviceApiSettings.BaseUri,
            Policy = new DiscoveryPolicy {RequireHttps = false}
        });

        if (disco.IsError)
        {
            throw disco.Exception;
        }

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

            return Response<bool>.Fail(errorDto.Errors, 400);
        }

        var userInfoRequest = new UserInfoRequest
        {
            Token = token.AccessToken,
            Address = disco.UserInfoEndpoint
        };

        var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

        if (userInfo.IsError)
        {
            throw userInfo.Exception;
        }

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationProperties = new AuthenticationProperties();

        authenticationProperties.StoreTokens(new List<AuthenticationToken>
        {
            new AuthenticationToken {Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
            new AuthenticationToken {Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
            }
        });

        authenticationProperties.IsPersistent = loginRequest.IsRemember;

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, authenticationProperties);

        return Response<bool>.Success(200);
    }

    public Task<TokenResponse> GetAccessTokenByRefreshToken()
    {
        throw new NotImplementedException();
    }

    public Task RevokeRefreshToken()
    {
        throw new NotImplementedException();
    }
}