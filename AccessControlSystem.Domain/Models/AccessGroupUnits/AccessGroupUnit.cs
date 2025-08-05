using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Shared.Attributs;

namespace AccessControlSystem.Domain.Models.AccessGroupUnits;

public class AccessGroupUnit : ISubscriptionEntity, IAuditable
{
    [CompositeKey]
    public int AccessGroupId { get; set; }

    [CompositeKey]
    public int UnitId { get; set; }

    public int SubscriptionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public AccessGroup AccessGroup { get; set; } = default!;
    public Unit Unit { get; set; } = default!;
}
