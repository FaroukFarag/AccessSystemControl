using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Units;

namespace AccessControlSystem.Domain.Models.Visitors;

public class Visitor : BaseModel<int>, ISubscriptionEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateTime VisitFrom { get; set; }
    public DateTime VisitTo { get; set; }
    public int UnitId { get; set; }
    public int SubscriptionId { get; set; }

    public Unit Unit { get; set; } = default!;
    public Subscription Subscription { get; set; } = default!;
}
