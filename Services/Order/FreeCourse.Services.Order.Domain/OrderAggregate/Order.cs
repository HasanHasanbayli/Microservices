using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    private readonly List<OrderItem> _orderItems;

    public Order()
    {
    }

    public Order(string buyerId, Address address)
    {
        _orderItems = new List<OrderItem>();
        CreatedDate = DateTime.Now;
        BuyerId = buyerId;
        Address = address;
    }

    public DateTime CreatedDate { get; }

    public Address Address { get; }

    public string BuyerId { get; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);

    public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
    {
        var existProduct = _orderItems.Any(x => x.ProductId == productId);

        if (!existProduct)
        {
            var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);

            _orderItems.Add(newOrderItem);
        }
    }
}