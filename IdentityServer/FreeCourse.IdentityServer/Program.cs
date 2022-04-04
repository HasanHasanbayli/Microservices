using FreeCourse.IdentityServer;
using FreeCourse.IdentityServer.Data;
using FreeCourse.IdentityServer.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllersWithViews();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var identityServerBuilder = services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<ApplicationUser>();

identityServerBuilder.AddDeveloperSigningCredential();

services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.ClientId = "copy client ID from Google here";
        options.ClientSecret = "copy client secret from Google here";
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseDatabaseErrorPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

try
{
     using (var scope = app.Services.CreateScope())
     {
         var serviceProvider = scope.ServiceProvider;

         var applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

         applicationDbContext.Database.Migrate();

         var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

         if (!userManager.Users.Any())
         {
             userManager.CreateAsync(new ApplicationUser
             {
                 UserName = "hasanhasanbayli", FirstName = "Hasan", LastName = "Hasanbayli",
                 Email = "hasanhasanbeyli@gmail.com"
             }, "Password12*").Wait();
         }
     }
}
catch (Exception e)
{
     Console.WriteLine(e);
}

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

app.Run();