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
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog_full_permission", "Full access for catalog API"),
            new ApiScope("catalog_full_permission", "Full access for Photo Stock API"),
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
                    {"catalog_full_permission", "catalog_full_permission", IdentityServerConstants.LocalApi.ScopeName}
            }
        };
}