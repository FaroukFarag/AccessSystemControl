using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Subscriptions;

namespace AccessControlSystem.Domain.Models.Visitors;

public class Visitor : BaseModel<int>, ISubscriptionEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AirfobUserId { get; set; }
    public int SiteId { get; set; }
    public int SubscriptionId { get; set; }

    public Subscription Subscription { get; set; } = default!;
    public IEnumerable<AccessGroup> AccessGroups { get; set; } = default!;
}
