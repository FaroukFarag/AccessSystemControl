using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Subscriptions;

namespace AccessControlSystem.Application.Interfaces.Subscriptions;

public interface ISubscriptionService : IBaseService<Subscription, SubscriptionDto, int>
{
    Task<ResultDto<IEnumerable<SubscriptionDto>>> GetAllAsync(string orderBy);
    Task<ResultDto<long>> GetSubscriptionsCountAsync();
}
