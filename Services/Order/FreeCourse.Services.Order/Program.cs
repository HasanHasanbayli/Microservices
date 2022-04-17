using System.IdentityModel.Tokens.Jwt;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        configure =>
        {
            configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");
        });
});

services.AddHttpContextAccessor();

services.AddScoped<ISharedIdentityService, SharedIdentityService>();

services.AddMediatR(typeof(FreeCourse.Services.Order.Application.Handlers.CreateOrderCommandHandler).Assembly);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "resource_order";
    options.RequireHttpsMetadata = false;
});

AuthorizationPolicy requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

WebApplication app = builder.Build();

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