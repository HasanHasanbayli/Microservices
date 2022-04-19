using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllersWithViews();

services.AddHttpContextAccessor();
services.AddHttpClient<IIdentityService, IdentityService>();
services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
services.Configure<ServiceApiSettings>(configuration.GetSection("ServiceApiSettings"));

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();