using IdentityServer4;
using IdentityServer4.Models;

namespace FreeCourse.IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("resource_catalog") { Scopes = { "catalog_full_permission" } },
            new ApiResource("resource_photo_stock") { Scopes = { "photo_stock_full_permission" } },
            new ApiResource("resource_basket") { Scopes = { "basket_full_permission" } },
            new ApiResource("resource_discount") { Scopes = { "discount_full_permission" } },
            new ApiResource("resource_order") { Scopes = { "order_full_permission" } },
            new ApiResource("resource_payment") { Scopes = { "payment_full_permission" } },
            new ApiResource("resource_gateway") { Scopes = { "gateway_full_permission" } },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new()
                { Name = "roles", DisplayName = "Roles", Description = "User roles", UserClaims = new[] { "role" } }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("catalog_full_permission", "Full access for Catalog API"),
            new ApiScope("photo_stock_full_permission", "Full access for Photo Stock API"),
            new ApiScope("basket_full_permission", "Full access for Basket API"),
            new ApiScope("discount_full_permission", "Full access for Discount API"),
            new ApiScope("order_full_permission", "Full access for Order API"),
            new ApiScope("payment_full_permission", "Full access for Payment API"),
            new ApiScope("gateway_full_permission", "Full access for Gateway"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            new Client
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMvcClient",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "catalog_full_permission",
                    "photo_stock_full_permission",
                    "gateway_full_permission",
                    IdentityServerConstants.LocalApi.ScopeName
                }
            },

            new Client
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMvcClientForUser",
                AllowOfflineAccess = true,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    "basket_full_permission",
                    "discount_full_permission",
                    "order_full_permission",
                    "payment_full_permission",
                    "gateway_full_permission",
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.LocalApi.ScopeName,
                    "roles"
                },
                AccessTokenLifetime = 1 * 60 * 60,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                RefreshTokenUsage = TokenUsage.ReUse
            }
        };
}