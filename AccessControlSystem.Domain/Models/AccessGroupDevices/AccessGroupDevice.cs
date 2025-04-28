using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Shared.Attributs;

namespace AccessControlSystem.Domain.Models.AccessGroupDevices;

public class AccessGroupDevice
{
    [CompositeKey]
    public int AccessGroupId { get; set; }
    [CompositeKey]
    public int DeviceId { get; set; }

    public AccessGroup AccessGroup { get; set; } = default!;
    public Device Device { get; set; } = default!;
}
