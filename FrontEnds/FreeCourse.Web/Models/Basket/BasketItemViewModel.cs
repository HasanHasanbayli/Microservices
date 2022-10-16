namespace FreeCourse.Web.Models.Basket;

public class BasketItemViewModel
{
    private decimal? DiscountAppliedPrice;
    public int Quantity { get; set; } = 1;

    public string CourseId { get; set; }
    public string CourseName { get; set; }

    public decimal Price { get; set; }

    public decimal GetCurrentPrice => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;

    public void AppliedDiscount(decimal discountPrice)
    {
        DiscountAppliedPrice = discountPrice;
    }
}