using AccessControlSystem.Application.Dtos.Visitors;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Visitors;

public class VisitorDtoValidator : AbstractValidator<VisitorDto>
{
    public VisitorDtoValidator()
    {
        RuleFor(v => v.Mobile)
            .MaximumLength(15)
            .WithMessage("Mobile number must not exceed 15 characters.")
            .Matches(@"^\+?[0-9]\d{1,14}$")
            .WithMessage("Mobile number must be a valid international phone number format.")
            .When(v => !string.IsNullOrEmpty(v.Mobile));

        RuleFor(v => v.StartDate)
           .LessThanOrEqualTo(v => v.EndDate)
           .WithMessage("Start date must be before or equal to end date.");
    }
}
