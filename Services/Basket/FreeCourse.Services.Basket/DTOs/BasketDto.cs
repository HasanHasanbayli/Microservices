namespace FreeCourse.Services.Basket.DTOs;

public class BasketDto
{
    public string UserId { get; set; }
    public string DiscountCode { get; set; }
    public List<BasketItemDto> BasketItem { get; set; }

    public decimal TotalPrice
    {
        get => BasketItem.Sum(x => x.Price * x.Quantity);
    }
}