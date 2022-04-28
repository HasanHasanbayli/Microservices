using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;

    public BasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
    {
        var response = await _httpClient.PostAsJsonAsync("basket", basketViewModel);
      
        return response.IsSuccessStatusCode;
    }

    public async Task<BasketViewModel> Get()
    {
        var response = await _httpClient.GetAsync("basket");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();

        return basketViewModel.Data;
    }

    public async Task<bool> Delete()
    {
        var result = await _httpClient.DeleteAsync("baskets");

        return result.IsSuccessStatusCode;
    }

    public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
    {
        var basket = await Get();

        if (basket != null && basket.BasketItem.Any(x => x.CourseId == basketItemViewModel.CourseId))
        {
            basket.BasketItem.Add(basketItemViewModel);
        }
        else
        {
            basket = new BasketViewModel();
            basket.BasketItem.Add(basketItemViewModel);
        }

        await SaveOrUpdate(basket);
    }

    public async Task<bool> DeleteBasketItem(string courseId)
    {
        var basket = await Get();

        if (basket == null)
        {
            return false;
        }

        var deleteBasketItem = basket.BasketItem.FirstOrDefault(x => x.CourseId == courseId);

        if (deleteBasketItem == null)
        {
            return false;
        }

        var deleteResult = basket.BasketItem.Remove(deleteBasketItem);

        if (!deleteResult)
        {
            return false;
        }

        if (!basket.BasketItem.Any())
        {
            basket.DiscountCode = null;
        }

        return await SaveOrUpdate(basket);
    }

    public Task<bool> ApplyDiscount(string discountCode)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CancelApplyDiscount()
    {
        throw new NotImplementedException();
    }
}