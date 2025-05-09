using AccessControlSystem.Domain.Enums.Subscriptions;

namespace AccessControlSystem.Domain.Interfaces.Services.Subscriptions;

public interface ISubscriptionValidationStrategy
{
    bool IsValid(int number);
    SubscriptionType HandledType { get; }
}
