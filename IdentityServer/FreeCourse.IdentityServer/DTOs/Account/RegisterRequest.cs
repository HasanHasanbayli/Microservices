using System.ComponentModel.DataAnnotations;

namespace FreeCourse.IdentityServer.DTOs.Account;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string UserName { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}