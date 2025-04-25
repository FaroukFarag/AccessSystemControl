using AccessControlSystem.Domain.Enums.Devices;
using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;

namespace AccessControlSystem.Domain.Models.Devices;

public class Device : BaseImageModel<int>
{
    public string Name { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public bool Active { get; set; }

    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
}
