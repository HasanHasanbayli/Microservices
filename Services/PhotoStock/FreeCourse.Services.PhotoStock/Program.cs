using FreeCourse.Services.PhotoStock.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;


services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "photo_stock_catalog";
    options.RequireHttpsMetadata = false;
});

services.AddControllers(
    options => { options.Filters.Add(new AuthorizeFilter()); }
);

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddScoped<IPhotoService, PhotoService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();