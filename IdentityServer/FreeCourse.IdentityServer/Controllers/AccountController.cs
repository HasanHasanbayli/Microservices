using FreeCourse.IdentityServer.DTOs.Account;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.IdentityServer.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : CustomBaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

        if (userWithSameUserName != null)
            return BadRequest(Response<NoContent>.Fail($"Username '{request.UserName}' is already taken.", 400));

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName
        };

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

        if (userWithSameEmail != null)
            return BadRequest(Response<NoContent>.Fail($"Email {request.Email} is already registered.", 400));

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));

        await _userManager.AddToRoleAsync(user, "BasicUser");

        return NoContent();
    }
}