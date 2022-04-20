using FreeCourse.Shared.Services;
using FreeCourse.Web.Handlers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllersWithViews();

services.AddHttpContextAccessor();

services.AddScoped<ResourceOwnerPasswordTokenHandler>();
services.AddScoped<ISharedIdentityService,SharedIdentityService>();

services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
services.Configure<ServiceApiSettings>(configuration.GetSection("ServiceApiSettings"));

var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

services.AddHttpClient<IIdentityService, IdentityService>();

services.AddHttpClient<ICatalogService, CatalogService>(options =>
{
    options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
});

services.AddHttpClient<IUserService, UserService>(options =>
{
    options.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
}).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.LoginPath = "/Auth/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromDays(60);
        options.SlidingExpiration = true;
        options.Cookie.Name = "udemywebcookie";
    });


WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();