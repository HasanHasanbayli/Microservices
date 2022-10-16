using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers;

[Authorize]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;
    private readonly ICatalogService _catalogService;

    public BasketController(ICatalogService catalogService, IBasketService basketService)
    {
        _catalogService = catalogService;
        _basketService = basketService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _basketService.Get());
    }

    public async Task<IActionResult> AddBasketItem(string courseId)
    {
        var course = await _catalogService.GetCourseById(courseId);

        BasketItemViewModel basketItem = new()
        {
            CourseId = course.Id,
            CourseName = course.Name,
            Price = course.Price
        };

        await _basketService.AddBasketItem(basketItem);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteBasketItem(string courseId)
    {
        await _basketService.DeleteBasketItem(courseId);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
    {
        if (!ModelState.IsValid)
        {
            TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
            return RedirectToAction(nameof(Index));
        }

        var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);

        TempData["discountStatus"] = discountStatus;

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> CancelApplyDiscount()
    {
        await _basketService.CancelApplyDiscount();

        return RedirectToAction(nameof(Index));
    }
}