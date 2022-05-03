using FluentValidation;

namespace FreeCourse.Web.Models.Discounts;

public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
{
    public DiscountApplyInputValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Discount coupon field cannot be empty");
    }
}