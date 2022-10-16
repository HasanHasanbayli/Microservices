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
        if (!ModelState.IsValid)
            return View();

        var categories = await _catalogService.GetAllCategoryAsync();

        ViewBag.categoryList = new SelectList(categories, "Id", "Name");

        courseCreateInput.UserId = _sharedIdentityService.GetUserId;

        await _catalogService.CreateCourseAsync(courseCreateInput);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(string id)
    {
        var course = await _catalogService.GetCourseById(id);

        var categories = await _catalogService.GetAllCategoryAsync();

        if (course == null) RedirectToAction(nameof(Index));

        ViewBag.categoryList = new SelectList(categories, "Id", "Name");

        CourseUpdateInput courseUpdateInput = new()
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            Price = course.Price,
            Feature = course.Feature,
            CategoryId = course.CategoryId,
            UserId = course.UserId,
            Picture = course.Picture
        };

        return View(courseUpdateInput);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
    {
        var categories = await _catalogService.GetAllCategoryAsync();

        ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);

        await _catalogService.UpdateCourseAsync(courseUpdateInput);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        await _catalogService.DeleteCourseAsync(id);

        return RedirectToAction(nameof(Index));
    }
}