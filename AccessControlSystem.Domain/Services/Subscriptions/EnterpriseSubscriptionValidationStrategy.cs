using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Services.Subscriptions;

namespace AccessControlSystem.Domain.Services.Subscriptions;

public class EnterpriseSubscriptionValidationStrategy : ISubscriptionValidationStrategy
{
    public SubscriptionType HandledType => SubscriptionType.Enterprise;

    public bool IsValid(int number)
    {
        return number >= 1 && number <= 20;
    }
}
