using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Devices;

public class DeviceDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public bool Active { get; set; }
}
