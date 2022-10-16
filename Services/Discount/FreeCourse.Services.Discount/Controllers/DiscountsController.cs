using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Discount.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountsController : CustomBaseController
{
    private readonly IDiscountService _discountService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
    {
        _discountService = discountService;
        _sharedIdentityService = sharedIdentityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return CreateActionResultInstance(await _discountService.GetAll());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var discount = await _discountService.GetById(id);

        return CreateActionResultInstance(discount);
    }

    [HttpGet]
    [Route("[action]/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var userId = _sharedIdentityService.GetUserId;

        var discount = await _discountService.GetByCodeAndUserid(code, userId);

        return CreateActionResultInstance(discount);
    }

    [HttpPost]
    public async Task<IActionResult> Save(Models.Discount discount)
    {
        return CreateActionResultInstance(await _discountService.Save(discount));
    }

    [HttpPut]
    public async Task<IActionResult> Update(Models.Discount discount)
    {
        return CreateActionResultInstance(await _discountService.Update(discount));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return CreateActionResultInstance(await _discountService.Delete(id));
    }
}