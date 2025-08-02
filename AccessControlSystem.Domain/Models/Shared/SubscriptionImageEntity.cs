using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Abstraction;

namespace AccessControlSystem.Domain.Models.Shared;

public class SubscriptionImageEntity : BaseImageModel<int>, ISubscriptionEntity
{
    public int SubscriptionId { get; set; }
}
