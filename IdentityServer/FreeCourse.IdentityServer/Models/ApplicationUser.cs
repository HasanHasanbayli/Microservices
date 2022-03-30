using Microsoft.AspNetCore.Identity;

namespace FreeCourse.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public string City { get; set; }
}