using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators;

public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateInput>
{
    public CourseUpdateInputValidator()
    {
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Namespace cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description field cannot be empty");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue)
                .WithMessage("Duration field cannot be empty.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Description field cannot be empty").InclusiveBetween(2, 6)
                .WithMessage("Wrong price format");
        }
    }
}