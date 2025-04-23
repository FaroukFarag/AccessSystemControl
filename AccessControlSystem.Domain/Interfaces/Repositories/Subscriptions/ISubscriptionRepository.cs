using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.Subscriptions;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;

public interface ISubscriptionRepository : IBaseRepository<Subscription, int>
{
}
