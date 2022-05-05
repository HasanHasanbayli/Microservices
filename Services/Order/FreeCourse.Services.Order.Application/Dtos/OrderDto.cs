using FreeCourse.Services.Order.Domain.OrderAggregate;

namespace FreeCourse.Services.Order.Application.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public AddressDto Address { get; set; }
    public string BuyerId { get; set; }
    private List<OrderItem> OrderItem { get; set; }
}