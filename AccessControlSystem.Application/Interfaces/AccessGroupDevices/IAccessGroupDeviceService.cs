using AccessControlSystem.Application.Dtos.AccessGroupDevices;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;

namespace AccessControlSystem.Application.Interfaces.AccessGroupDevices;

public interface IAccessGroupDeviceService :
    IBaseService<
        AccessGroupDevice,
        AccessGroupDeviceDto,
        (int AccessGroupId, int DeviceId)>
{
}
