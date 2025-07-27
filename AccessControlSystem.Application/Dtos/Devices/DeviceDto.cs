using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Enums.Devices;

namespace AccessControlSystem.Application.Dtos.Devices;

public class DeviceDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public string Serial { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public int SiteId { get; set; } = default!;
    public int? AirfobDeviceId { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? DeviceTypeName { get; set; }
    public bool Active { get; set; }
    public int SubscriptionId { get; set; }
}
