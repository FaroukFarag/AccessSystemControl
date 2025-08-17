using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Subscriptions;

namespace AccessControlSystem.Application.Interfaces.Subscriptions;

public interface ISubscriptionService : IBaseService<SubscriptionDto,
    SubscriptionDto, SubscriptionDto, SubscriptionDto, Subscription, int>
{
    Task<ResultDto<IEnumerable<SubscriptionDto>>> GetAllAsync(string orderBy);
    Task<ResultDto<long>> GetSubscriptionsCountAsync(bool isLastMonth = false);
    Task<ResultDto<SubscriptionDto>> UpgradeAsync(UpgradeSubscriptionDto newSubscriptionDto);
}
