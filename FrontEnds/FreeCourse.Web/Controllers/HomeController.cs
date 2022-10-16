using System.Diagnostics;
using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _catalogService.GetAllCourseAsync());
    }

    public async Task<IActionResult> Detail(string id)
    {
        return View(await _catalogService.GetCourseById(id));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
            return RedirectToAction(nameof(AuthController.Logout), "Auth");

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}