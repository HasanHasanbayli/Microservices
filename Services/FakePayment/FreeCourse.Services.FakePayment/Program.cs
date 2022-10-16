using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddMassTransit(x =>
{
    // Default Port: 5672
    x.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(configuration["RabbitMQUrl"], "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });
    });
});

services.AddMassTransitHostedService();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "resource_payment";
    options.RequireHttpsMetadata = false;
});

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

services.AddControllers(options => { options.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();