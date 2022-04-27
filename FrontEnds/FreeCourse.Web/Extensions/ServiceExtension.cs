using FreeCourse.Web.Handlers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Extensions;

public static class ServiceExtension
{
    public static void AddHttpClientService(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

        services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

        services.AddHttpClient<IIdentityService, IdentityService>();

        services.AddHttpClient<IUserService, UserService>(options =>
        {
            options.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        services.AddHttpClient<ICatalogService, CatalogService>(options =>
        {
            options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

        services.AddHttpClient<IPhotoStockService, PhotoStockService>(options =>
        {
            options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

        services.AddHttpClient<IBasketService, BasketService>(options =>
        {
            options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
    }
}