using System.IdentityModel.Tokens.Jwt;
using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddHttpContextAccessor();

services.AddScoped<ISharedIdentityService, SharedIdentityService>();
services.AddScoped<IDiscountService, DiscountService>();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "resource_discount";
    options.RequireHttpsMetadata = false;
});

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

services.AddControllers(options => { options.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); });

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