using AccessControlSystem.Domain.Models.Shared;

namespace AccessControlSystem.Domain.Models.Cards;

public class Card : SubscriptionEntity
{
    public string Name { get; set; } = default!;
    public int SubscriptionId { get; set; }
    public bool Active { get; set; }
}
