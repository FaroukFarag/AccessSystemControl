using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Dtos.Users;

namespace AccessControlSystem.Application.Dtos.AccessGroups;

public class AccessGroupDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;

    public UserDto? Owner { get; set; }
    public IEnumerable<DeviceDto>? Devices { get; set; }
}
