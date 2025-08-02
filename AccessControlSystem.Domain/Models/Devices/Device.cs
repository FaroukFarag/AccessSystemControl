using AccessControlSystem.Domain.Enums.Devices;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Subscriptions;

namespace AccessControlSystem.Domain.Models.Devices;

public class Device : SubscriptionImageEntity
{
    public string Name { get; set; } = default!;
    public string Serial { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public int? AirfobDeviceId { get; set; }
    public bool Active { get; set; }

    public Subscription Subscription { get; set; } = default!;
    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
}
