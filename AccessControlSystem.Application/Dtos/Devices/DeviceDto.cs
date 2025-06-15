using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Enums.Devices;

namespace AccessControlSystem.Application.Dtos.Devices;

public class DeviceDto : BaseImageModelDto<int>
{
    public string? Name { get; set; }
    public string? MacAddress { get; set; }
    public DeviceType DeviceType { get; set; }
    public string? DeviceTypeName { get; set; }
    public bool Active { get; set; }
    public int SubscriptionId { get; set; }
}
