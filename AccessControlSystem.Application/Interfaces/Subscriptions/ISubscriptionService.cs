using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Subscriptions;

namespace AccessControlSystem.Application.Interfaces.Subscriptions;

public interface ISubscriptionService : IBaseService<Subscription, SubscriptionDto, int>
{
}
