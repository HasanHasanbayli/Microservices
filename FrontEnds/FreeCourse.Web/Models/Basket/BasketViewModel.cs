namespace FreeCourse.Web.Models.Basket;

public class BasketViewModel
{
    private List<BasketItemViewModel> _basketItem;

    public BasketViewModel()
    {
        _basketItem = new List<BasketItemViewModel>();
    }

    public string UserId { get; set; }

    public string DiscountCode { get; set; }

    public int? DiscountRate { get; set; }

    public List<BasketItemViewModel> BasketItem
    {
        get
        {
            if (HasDiscount)
                _basketItem.ForEach(x =>
                {
                    var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                    x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
                });

            return _basketItem;
        }
        set => _basketItem = value;
    }

    public decimal TotalPrice => _basketItem.Sum(x => x.GetCurrentPrice);

    public bool HasDiscount => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;

    public void CancelDiscount()
    {
        DiscountCode = null;
        DiscountRate = null;
    }

    public void ApplyDiscount(string code, int rate)
    {
        DiscountCode = code;
        DiscountRate = rate;
    }
}