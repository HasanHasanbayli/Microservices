using System.IdentityModel.Tokens.Jwt;
using FreeCourse.Services.Order.Application.Consumers;
using FreeCourse.Services.Order.Application.Handlers;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        configure => { configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure"); });
});

services.AddMassTransit(x =>
{
    x.AddConsumer<CreateOrderMessageCommandConsumers>();
    x.AddConsumer<CourseNameChangedEventConsumer>();

    // Default Port: 5672
    x.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(configuration["RabbitMQUrl"], "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });

        configurator.ReceiveEndpoint("create-order-service", e =>
        {
            e.ConfigureConsumer<CreateOrderMessageCommandConsumers>(context);
        });
        
        configurator.ReceiveEndpoint("course-name-changed-event-order-service", e =>
        {
            e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
        });
    });
});

services.AddMassTransitHostedService();

services.AddHttpContextAccessor();

services.AddScoped<ISharedIdentityService, SharedIdentityService>();

services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "resource_order";
    options.RequireHttpsMetadata = false;
});

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

services.AddControllers(options => { options.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); });

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

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