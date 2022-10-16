using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Services;

public class ClientCredentialTokenService : IClientCredentialTokenService
{
    private readonly IClientAccessTokenCache _clientAccessTokenCache;
    private readonly ClientSettings _clientSettings;
    private readonly HttpClient _httpClient;
    private readonly ServiceApiSettings _serviceApiSettings;

    public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings,
        IOptions<ClientSettings> clientSettings,
        IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
    {
        _serviceApiSettings = serviceApiSettings.Value;
        _clientSettings = clientSettings.Value;
        _clientAccessTokenCache = clientAccessTokenCache;
        _httpClient = httpClient;
    }

    public async Task<string> GetToken()
    {
        var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", default!);

        if (currentToken != null) return currentToken.AccessToken;

        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceApiSettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError) throw disco.Exception;

        ClientCredentialsTokenRequest clientCredentialTokenRequest = new()
        {
            ClientId = _clientSettings.WebClient.ClientId,
            ClientSecret = _clientSettings.WebClient.ClientSecret,
            Address = disco.TokenEndpoint
        };

        var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

        if (newToken.IsError) throw newToken.Exception;

        await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, default!);


        return newToken.AccessToken;
    }
}