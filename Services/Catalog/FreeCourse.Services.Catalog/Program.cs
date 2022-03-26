using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ICourseService, CourseService>();

services.AddAutoMapper(typeof(Program));

services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

services.AddSingleton<IDataBaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();