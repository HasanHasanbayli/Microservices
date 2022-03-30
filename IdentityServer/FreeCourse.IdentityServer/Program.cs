using FreeCourse.IdentityServer.Data;
using FreeCourse.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
                    {UserName = "HasanHasanbayli", Email = "HasanHasanbeyli@gmail.com", City = "Baku"}, "Password123@")
                .Wait();
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();