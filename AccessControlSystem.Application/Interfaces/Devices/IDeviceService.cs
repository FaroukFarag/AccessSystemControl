using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Devices;

namespace AccessControlSystem.Application.Interfaces.Devices;

public interface IDeviceService : IBaseService<Device, DeviceDto, int>
{
    Task<ResultDto<IEnumerable<DeviceDto>>> GetAllAsync(string orderBy);
    Task<ResultDto<IEnumerable<DeviceDto>>> GetAvailableDevicesForAccessGroupAsync(int accessGroupId);
    Task<ResultDto<long>> GetDevicesCountAsync();
}
