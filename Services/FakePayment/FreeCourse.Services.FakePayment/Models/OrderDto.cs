namespace FreeCourse.Services.FakePayment.Models;

public class OrderDto
{
    public OrderDto()
    {
        OrderItems = new List<OrderItem>();
    }

    public string BuyerId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public AddressDto Address { get; set; }
}