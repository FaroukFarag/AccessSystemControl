using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Constants.Devices;
using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Devices;

public class DeviceService(
    IDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ISubscriptionService subscriptionService,
    IImageService imageService) :
    BaseService<Device, DeviceDto, int>(repository, unitOfWork, mapper), IDeviceService
{
    private readonly IDeviceRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ISubscriptionService _subscriptionService = subscriptionService;
    private readonly IImageService _imageService = imageService;

    public override async Task<DeviceDto> CreateAsync(DeviceDto deviceDto)
    {
        deviceDto.ImagePath = await _imageService
            .SaveImageAsync(deviceDto.ImageFile, DeviceConstants.SubFolder);

        return await base.CreateAsync(deviceDto);
    }

    public override async Task<DeviceDto> GetAsync(int id)
    {
        var device = await base.GetAsync(id);

        return device;
    }

    public override async Task<IEnumerable<DeviceDto>> GetAllAsync()
    {
        var devices = await base.GetAllAsync();

        return devices;
    }

    public async Task<IEnumerable<DeviceDto>> GetAvailableDevicesForSubscription(int subscriptionId)
    {
        var availableDevices = await _repository.GetAllAsync(
            new BaseSpecification<Device>
            {
                Criteria = d => !d.SubscriptionsDevices.Any(sd => sd.SubscriptionId == subscriptionId)
            });

        return _mapper.Map<IReadOnlyList<DeviceDto>>(availableDevices);
    }

    public override async Task<DeviceDto> UpdateAsync(DeviceDto newDeviceDto)
    {
        var device = await GetAsync(newDeviceDto.Id);

        _imageService.DeleteImageAsync(device.ImagePath!);

        newDeviceDto.ImagePath = await _imageService
            .SaveImageAsync(newDeviceDto.ImageFile, DeviceConstants.SubFolder);

        return await base.UpdateAsync(newDeviceDto);
    }

    public override async Task<DeviceDto> DeleteAsync(int id)
    {
        var device = await GetAsync(id);

        _imageService.DeleteImageAsync(device.ImagePath!);

        return await base.DeleteAsync(id);
    }
}
