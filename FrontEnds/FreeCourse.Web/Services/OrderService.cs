using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class OrderService : IOrderService
{
    private readonly IPaymentService _paymentService;
    private readonly HttpClient _httpClient;
    private readonly IBasketService _basketService;

    public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService)
    {
        _paymentService = paymentService;
        _httpClient = httpClient;
        _basketService = basketService;
    }

    public Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
    {
        throw new NotImplementedException();
    }

    public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderViewModel>> GetOrder()
    {
        throw new NotImplementedException();
    }
}