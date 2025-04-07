using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Models.Units;

public class Unit : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public int UserId { get; set; }
    public int SubscriptionId { get; set; }

    public User User { get; set; } = default!;
    public Subscription Subscription { get; set; } = default!;
    public IEnumerable<Device> Devices { get; set; } = default!;
}
