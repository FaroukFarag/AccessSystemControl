using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Services.Subscriptions;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Subscriptions;

public class SubscriptionDtoValidator : AbstractValidator<SubscriptionDto>
{
    private readonly SubscriptionValidationStrategyFactory _strategyFactory;

    public SubscriptionDtoValidator(SubscriptionValidationStrategyFactory strategyFactory)
    {
        _strategyFactory = strategyFactory;

        RuleFor(s => s.CustomerName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(s => s.DeviceNumber)
            .Must((dto, deviceNumber) => ValidateDeviceNumberAgainstSubscriptionType(dto.SubscriptionType, deviceNumber))
            .WithMessage("Device number is not valid for the selected subscription type");
    }

    private bool ValidateDeviceNumberAgainstSubscriptionType(SubscriptionType type, int deviceNumber)
    {
        var strategy = _strategyFactory.GetStrategy(type);

        return strategy.IsValid(deviceNumber);
    }
}
