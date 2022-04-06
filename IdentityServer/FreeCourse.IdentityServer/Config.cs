using IdentityServer4;
using IdentityServer4.Models;

namespace FreeCourse.IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("resource_catalog") {Scopes = {"catalog_full_permission"}},
            new ApiResource("photo_stock_catalog") {Scopes = {"photo_stock_full_permission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
                {Name = "roles", DisplayName = "Roles", Description = "User roles", UserClaims = new[] {"role"}}
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog_full_permission", "Full access for catalog API"),
            new ApiScope("photo_stock_full_permission", "Full access for Photo Stock API"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMvcClient",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "catalog_full_permission", "photo_stock_full_permission", IdentityServerConstants.LocalApi.ScopeName
                }
            },

            new Client
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMvcClientForUser",
                AllowOfflineAccess = true,
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OfflineAccess,
                },
                AccessTokenLifetime = 1 * 60 * 60,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = (int) (DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                RefreshTokenUsage = TokenUsage.ReUse
            }
        };
}