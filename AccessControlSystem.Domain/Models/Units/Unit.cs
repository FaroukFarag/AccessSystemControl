using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Models.Units;

public class Unit : BaseImageModel<int>
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }
    public decimal Area { get; set; }
    public int CardNumber { get; set; }
    public int UserId { get; set; }
    public int SubscriptionId { get; set; }

    public User User { get; set; } = default!;
    public Subscription Subscription { get; set; } = default!;
    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
}
