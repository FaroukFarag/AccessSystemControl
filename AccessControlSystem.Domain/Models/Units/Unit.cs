using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Models.Visitors;

namespace AccessControlSystem.Domain.Models.Units;

public class Unit : SubscriptionImageEntity
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }
    public decimal Area { get; set; }
    public int CardNumber { get; set; }
    public int? OwnerId { get; set; }

    public Subscription Subscription { get; set; } = default!;
    public User? Owner { get; set; }
    public IEnumerable<AccessGroupUnit> AccessGroupUnits { get; set; } = default!;
    public IEnumerable<Visitor> Visitors { get; set; } = default!;
}
