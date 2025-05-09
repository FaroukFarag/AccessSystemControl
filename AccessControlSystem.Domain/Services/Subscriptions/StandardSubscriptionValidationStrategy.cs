using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Services.Subscriptions;

namespace AccessControlSystem.Domain.Services.Subscriptions;

public class StandardSubscriptionValidationStrategy : ISubscriptionValidationStrategy
{
    public SubscriptionType HandledType => SubscriptionType.Standard;

    public bool IsValid(int number)
    {
        return number >= 1 && number <= 5;
    }
}
