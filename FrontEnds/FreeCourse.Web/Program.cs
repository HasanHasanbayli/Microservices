using FluentValidation.AspNetCore;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Extensions;
using FreeCourse.Web.Handlers;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using FreeCourse.Web.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllersWithViews().AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>());

services.AddHttpContextAccessor();
services.AddAccessTokenManagement();

services.AddScoped<ResourceOwnerPasswordTokenHandler>();
services.AddScoped<ClientCredentialTokenHandler>();
services.AddScoped<ISharedIdentityService, SharedIdentityService>();
services.AddScoped<IPhotoStockService, PhotoStockService>();

services.AddSingleton<PhotoHelper>();

services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
services.Configure<ServiceApiSettings>(configuration.GetSection("ServiceApiSettings"));

services.AddHttpClientService(configuration);

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.LoginPath = "/Auth/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromDays(60);
        options.SlidingExpiration = true;
        options.Cookie.Name = "udemywebcookie";
    });

var app = builder.Build();

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