using AccessControlSystem.Application.Dtos.Subscriptions;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Subscriptions;

public class SubscriptionDtoValidator : AbstractValidator<SubscriptionDto>
{
    public SubscriptionDtoValidator()
    {
        RuleFor(s => s.CustomerName)
            .NotEmpty()
            .MaximumLength(50);
    }
}