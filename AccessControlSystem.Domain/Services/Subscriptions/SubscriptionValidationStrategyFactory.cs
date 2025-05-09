using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Services.Subscriptions;

namespace AccessControlSystem.Domain.Services.Subscriptions;

public class SubscriptionValidationStrategyFactory
{
    private readonly Dictionary<SubscriptionType, ISubscriptionValidationStrategy> _strategies;

    public SubscriptionValidationStrategyFactory()
    {
        _strategies = new Dictionary<SubscriptionType, ISubscriptionValidationStrategy>
        {
            { SubscriptionType.Standard, new StandardSubscriptionValidationStrategy() },
            { SubscriptionType.Premium, new PremiumSubscriptionValidationStrategy() },
            { SubscriptionType.Enterprise, new EnterpriseSubscriptionValidationStrategy() }
        };
    }

    public ISubscriptionValidationStrategy GetStrategy(SubscriptionType type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
        {
            return strategy;
        }

        throw new ArgumentException($"No validation strategy found for subscription type {type}");
    }
}
