using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Services.Subscriptions;

namespace AccessControlSystem.Domain.Services.Subscriptions;

public class PremiumSubscriptionValidationStrategy : ISubscriptionValidationStrategy
{
    public SubscriptionType HandledType => SubscriptionType.Premium;

    public bool IsValid(int number)
    {
        return number >= 1 && number > 20;
    }
}
