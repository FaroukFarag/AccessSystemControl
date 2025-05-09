using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Devices;

namespace AccessControlSystem.Application.Interfaces.Devices;

public interface IDeviceService : IBaseService<Device, DeviceDto, int>
{
    Task<IEnumerable<DeviceDto>> GetAvailableDevicesForSubscription(int subscriptionId);
    Task<IEnumerable<DeviceDto>> GetAvailableDevicesForAccessGroup(int accessGroupId);
}
