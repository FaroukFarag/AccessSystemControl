using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Models.Cards;

public class Card : SubscriptionEntity
{
    public int OwnerId { get; set; }
    public int AirfobUserId { get; set; }

    public User Owner { get; set; } = default!;
}
