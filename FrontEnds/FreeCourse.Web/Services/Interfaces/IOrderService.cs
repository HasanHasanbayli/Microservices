using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interfaces;

public interface IOrderService
{
    /// <summary>
    ///     Synchronous communication
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns>
    Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);

    /// <summary>
    ///     Asynchronous communication
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns>
    Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);

    Task<List<OrderViewModel>> GetOrder();
}