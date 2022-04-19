using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSchema", options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

builder.WebHost.ConfigureAppConfiguration((context, configurationBuilder) =>
{
    configurationBuilder.AddJsonFile($"configuration.{context.HostingEnvironment.EnvironmentName.ToLower()}.json")
        .AddEnvironmentVariables();
});

builder.Services.AddOcelot();

var app = builder.Build();

app.UseOcelot();

app.Run();