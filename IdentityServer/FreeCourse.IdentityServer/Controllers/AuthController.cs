using FreeCourse.IdentityServer.DTOs;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.DTOs;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FreeCourse.IdentityServer.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.UserName,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));

        return NoContent();
    }

    [HttpGet("getuser")]
    public async Task<IActionResult> GetUser()
    {
        var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

        if (userClaim == null)
            return BadRequest();

        var user = await _userManager.FindByIdAsync(userClaim.Value);

        if (user == null)
            return BadRequest();

        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.FirstName,
            user.LastName
        });
    }
}