using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeCourse.Web.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
    {
        _catalogService = catalogService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId));
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _catalogService.GetAllCategoryAsync();

        ViewBag.categoryList = new SelectList(categories, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        
        ViewBag.categoryList = new SelectList(categories, "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View();
        }

        courseCreateInput.UserId = _sharedIdentityService.GetUserId;

        var a = await _catalogService.CreateCourseAsync(courseCreateInput);

        return RedirectToAction(nameof(Index));
    }
}