using FreeCourse.Web.Models.Basket;

namespace FreeCourse.Web.Services.Interfaces;

public interface IBasketService
{
    Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
    Task<BasketViewModel> Get();
    Task<bool> Delete();
    Task AddBasketItem(BasketItemViewModel basketItemViewModel);
    Task<bool> DeleteBasketItem(string courseId);
    Task<bool> ApplyDiscount(string discountCode);
    Task<bool> CancelApplyDiscount();
}