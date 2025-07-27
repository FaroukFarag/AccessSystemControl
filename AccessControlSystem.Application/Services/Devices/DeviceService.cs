using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Application.Services.Shared;
using AccessControlSystem.Domain.Constants.Devices;
using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Specifications.Absraction;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Devices;

public class DeviceService(
    IDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IAirfobDeviceService airfobDeviceService) : BaseService<Device, DeviceDto, int>(repository, unitOfWork, mapper), IDeviceService
{
    private readonly IDeviceRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IAirfobDeviceService _airfobDeviceService = airfobDeviceService;

    public override async Task<ResultDto<DeviceDto>> CreateAsync(DeviceDto deviceDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Device",
            action: async () =>
            {
                deviceDto.ImagePath = await _imageService.SaveImageAsync(
                    deviceDto.ImageFile!,
                    DeviceConstants.SubFolder);

                var airfobRequest = new CreateDevicesRequest
                {
                    Devices = [_mapper.Map<CreateDeviceRequest>(deviceDto)]
                };

                var airfobResponse = await _airfobDeviceService.CreateDevicesAsync(airfobRequest);

                if (!airfobResponse.Succeeded)
                {
                    _imageService.DeleteImage(deviceDto.ImagePath);

                    throw new InvalidOperationException("Failed to create device");
                }

                return (await base.CreateAsync(deviceDto)).ResultData;
            });
    }

    public async Task<ResultDto<IEnumerable<DeviceDto>>> GetAvailableDevicesForAccessGroup(int accessGroupId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Available Devices for Access Group",
            action: async () =>
            {
                var spec = new BaseSpecification<Device>
                {
                    Criteria = d => !d.AccessGroupDevices.Any(agd => agd.AccessGroupId == accessGroupId)
                };

                var devices = await _repository.GetAllAsync(spec);

                return _mapper.Map<IEnumerable<DeviceDto>>(devices);
            });
    }

    public override async Task<ResultDto<DeviceDto>> UpdateAsync(DeviceDto newDeviceDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Device",
            action: async () =>
            {
                var existingDevice = await base.GetAsync(newDeviceDto.Id);

                _imageService.DeleteImage(existingDevice.ResultData?.ImagePath!);

                newDeviceDto.ImagePath = await _imageService.SaveImageAsync(
                    newDeviceDto.ImageFile!,
                    DeviceConstants.SubFolder);

                return (await base.UpdateAsync(newDeviceDto)).ResultData;
            });
    }

    public override async Task<ResultDto<DeviceDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Device",
            action: async () =>
            {
                var device = await base.GetAsync(id);

                _imageService.DeleteImage(device.ResultData?.ImagePath!);

                return (await base.DeleteAsync(id)).ResultData;
            });
    }
}
