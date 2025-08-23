using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Devices;

namespace AccessControlSystem.Application.Interfaces.Devices;

public interface IDeviceService : IBaseService<DeviceDto, DeviceDto, DeviceDto,
    DeviceDto, Device, int>
{
    Task<ResultDto<IEnumerable<DeviceDto>>> GetAllAsync(string orderBy);
    Task<ResultDto<IEnumerable<DeviceDto>>> GetAvailableDevicesForAccessGroupAsync(int accessGroupId);
    Task<ResultDto<IEnumerable<DeviceTrafficDto>>> GetDevicesTrafficAsync();
    Task<ResultDto<IEnumerable<SubscriptionDeviceDto>>> GetSubscriptionDevicesAsync();
    Task<ResultDto<long>> GetDevicesCountAsync(bool isLastMonth = false);
}
