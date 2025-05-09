using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.Devices;

namespace AccessControlSystem.Application.Dtos.AccessGroups;

public class AccessGroupDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;

    public IEnumerable<DeviceDto>? Devices { get; set; }
}
