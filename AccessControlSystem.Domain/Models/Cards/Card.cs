using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Units;

namespace AccessControlSystem.Domain.Models.Cards;

public class Card : SubscriptionEntity
{
    public int UnitId { get; set; }
    public int AirfobUserId { get; set; }

    public Unit Unit { get; set; } = default!;
}
