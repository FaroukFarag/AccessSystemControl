using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Enums.Devices;

namespace AccessControlSystem.Application.Dtos.Devices;

public class DeviceDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public string? DeviceTypeName { get; set; }
    public bool Active { get; set; }
    public int SubscriptionId { get; set; }
}
