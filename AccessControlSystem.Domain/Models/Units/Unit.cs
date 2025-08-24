using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Models.Units;

public class Unit : SubscriptionImageEntity
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }
    public decimal Area { get; set; }
    public int CardNumber { get; set; }
    public string? AssignedOwner { get; set; }

    public Subscription Subscription { get; set; } = default!;
    public IEnumerable<User> Owners { get; set; } = default!;
    public IEnumerable<AccessGroupUnit> AccessGroupUnits { get; set; } = default!;
    public virtual ICollection<Card> Cards { get; set; } = default!;
}
