using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Abstraction;

namespace AccessControlSystem.Domain.Models.Shared;

public class SubscriptionEntity : BaseModel<int>, ISubscriptionEntity
{
    public int SubscriptionId { get; set; }
}
