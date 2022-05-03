using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators;

public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
{
    public CourseCreateInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Namespace cannot be empty");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description field cannot be empty");
        RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue)
            .WithMessage("Duration field cannot be empty");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price field cannot be empty").ScalePrecision(2, 6)
            .WithMessage("Wrong currency format");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Choose the category");
    }
}