using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Application.Services.Shared;
using AccessControlSystem.Domain.Models.Devices;

namespace AccessControlSystem.Application.Interfaces.Devices;

public interface IDeviceService : IBaseService<Device, DeviceDto, int>
{
    Task<ResultDto<IEnumerable<DeviceDto>>> GetAvailableDevicesForAccessGroup(int accessGroupId);
}
