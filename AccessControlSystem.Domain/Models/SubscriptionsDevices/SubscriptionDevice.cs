using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Shared.Attributs;

namespace AccessControlSystem.Domain.Models.SubscriptionsDevices;

public class SubscriptionDevice
{
    [CompositeKey]
    public int SubscriptionId { get; set; }
    [CompositeKey]
    public int DeviceId { get; set; }

    public Subscription Subscription { get; set; } = default!;
    public Device Device { get; set; } = default!;
}
