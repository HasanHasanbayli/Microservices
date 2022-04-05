using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = configuration["IdentityServerUrl"];
    options.Audience = "resource_catalog";
    options.RequireHttpsMetadata = false;
});

services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddAutoMapper(typeof(Program));

services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ICourseService, CourseService>();

services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

services.AddSingleton<IDataBaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();