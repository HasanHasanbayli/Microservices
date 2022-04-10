using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Basket.Services;

public interface IBasketService
{
    Task<Response<BasketDto>> GetBasket(string userId);

    Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);

    Task<Response<bool>> Delete(string userId);
}