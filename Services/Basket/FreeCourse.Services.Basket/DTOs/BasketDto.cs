namespace FreeCourse.Services.Basket.DTOs;

public class BasketDto
{
    public string? UserId { get; set; }
    public string? DiscountCode { get; set; }
    public int? DiscountRate { get; set; }

    public List<BasketItemDto>? BasketItem { get; set; }

    public decimal TotalPrice => BasketItem.Sum(x => x.Price * x.Quantity);
}