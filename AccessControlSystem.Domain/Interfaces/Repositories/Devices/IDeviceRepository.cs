using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.Devices;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Devices;

public interface IDeviceRepository : IBaseRepository<Device, int>
{
}
