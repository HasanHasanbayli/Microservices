using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers;

public class AuthController : Controller
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return View();

        var response = await _identityService.SignIn(loginRequest);

        if (response.IsSuccessful)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        response.Errors.ForEach(error => { ModelState.AddModelError(string.Empty, error); });

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        await _identityService.RevokeRefreshToken();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}