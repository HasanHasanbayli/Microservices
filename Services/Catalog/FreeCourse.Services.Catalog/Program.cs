using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "resource_catalog";
    options.RequireHttpsMetadata = false;
});

services.AddControllers(
    options => { options.Filters.Add(new AuthorizeFilter()); }
);

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

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddAutoMapper(typeof(Program));

services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ICourseService, CourseService>();

services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

services.AddSingleton<IDataBaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

    if (!categoryService.GetAllAsync().Result.Data.Any())
    {
        categoryService.CreateAsync(new CategoryCreateDTO { Name = "Asp.net Core Course" }).Wait();
        categoryService.CreateAsync(new CategoryCreateDTO { Name = "Asp.net Core Web API Course" }).Wait();
    }
}

app.Run();