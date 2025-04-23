using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;

namespace AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupDevices;

public interface IAccessGroupDeviceRepository :
    IBaseRepository<AccessGroupDevice, (int AccessGroupId, int DeviceId)>
{
}
