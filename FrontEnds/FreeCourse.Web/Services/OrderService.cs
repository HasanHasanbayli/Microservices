using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class OrderService : IOrderService
{
    private readonly IBasketService _basketService;
    private readonly HttpClient _httpClient;
    private readonly IPaymentService _paymentService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService,
        ISharedIdentityService sharedIdentityService)
    {
        _paymentService = paymentService;
        _httpClient = httpClient;
        _basketService = basketService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
    {
        var basket = await _basketService.Get();

        PaymentInfoInput paymentInfoInput = new()
        {
            CardName = checkoutInfoInput.CardName,
            CardNumber = checkoutInfoInput.CardNumber,
            Expiration = checkoutInfoInput.Expiration,
            CVV = checkoutInfoInput.CVV,
            TotalPrice = basket.TotalPrice
        };

        var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

        if (!responsePayment) return new OrderCreatedViewModel { Error = "Payment not received", IsSuccessful = false };

        OrderCreateInput orderCreateInput = new()
        {
            BuyerId = _sharedIdentityService.GetUserId,
            Address = new AddressCreateInput
            {
                Province = checkoutInfoInput.Province,
                District = checkoutInfoInput.District,
                Street = checkoutInfoInput.Street,
                Line = checkoutInfoInput.Line,
                ZipCode = checkoutInfoInput.ZipCode
            }
        };

        basket.BasketItem.ForEach(x =>
        {
            OrderItemCreateInput orderItem = new()
            {
                ProductId = x.CourseId,
                Price = x.GetCurrentPrice,
                PictureUrl = "",
                ProductName = x.CourseName
            };

            orderCreateInput.OrderItems.Add(orderItem);
        });

        var response = await _httpClient.PostAsJsonAsync("orders", orderCreateInput);

        if (!response.IsSuccessStatusCode)
            return new OrderCreatedViewModel { Error = "Failed to create order", IsSuccessful = false };

        var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

        orderCreatedViewModel.Data.IsSuccessful = true;

        await _basketService.Delete();

        return orderCreatedViewModel.Data;
    }

    public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
    {
        var basket = await _basketService.Get();

        OrderCreateInput orderCreateInput = new()
        {
            BuyerId = _sharedIdentityService.GetUserId,
            Address = new AddressCreateInput
            {
                Province = checkoutInfoInput.Province,
                District = checkoutInfoInput.District,
                Street = checkoutInfoInput.Street,
                Line = checkoutInfoInput.Line,
                ZipCode = checkoutInfoInput.ZipCode
            }
        };

        basket.BasketItem.ForEach(x =>
        {
            OrderItemCreateInput orderItem = new()
            {
                ProductId = x.CourseId,
                Price = x.GetCurrentPrice,
                PictureUrl = "",
                ProductName = x.CourseName
            };

            orderCreateInput.OrderItems.Add(orderItem);
        });

        PaymentInfoInput paymentInfoInput = new()
        {
            CardName = checkoutInfoInput.CardName,
            CardNumber = checkoutInfoInput.CardNumber,
            Expiration = checkoutInfoInput.Expiration,
            CVV = checkoutInfoInput.CVV,
            TotalPrice = basket.TotalPrice,
            Order = orderCreateInput
        };

        var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

        if (!responsePayment) return new OrderSuspendViewModel { Error = "Payment not received", IsSuccessful = false };

        await _basketService.Delete();

        return new OrderSuspendViewModel { IsSuccessful = true };
    }

    public async Task<List<OrderViewModel>> GetOrder()
    {
        var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

        return response.Data;
    }
}