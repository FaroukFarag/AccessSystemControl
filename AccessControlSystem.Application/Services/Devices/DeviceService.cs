using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Constants.Devices;
using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Devices;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Devices;

public class DeviceService(
    IDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) :
    BaseService<Device, DeviceDto, int>(repository, unitOfWork, mapper), IDeviceService
{
    private readonly IImageService _imageService = imageService;

    public override async Task<DeviceDto> CreateAsync(DeviceDto deviceDto)
    {
        deviceDto.ImagePath = await _imageService
            .SaveImageAsync(deviceDto.ImageFile, DeviceConstants.SubFolder);

        return await base.CreateAsync(deviceDto);
    }

    public override async Task<IEnumerable<DeviceDto>> GetAllAsync()
    {
        var devices = await base.GetAllAsync();

        foreach (var device in devices)
        {
            if (!string.IsNullOrEmpty(device.ImagePath) && File.Exists(device.ImagePath))
            {
                var imageBytes = await File.ReadAllBytesAsync(device.ImagePath);

                device.ImageEncode = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        return devices;
    }

    public override async Task<DeviceDto> Update(DeviceDto newDeviceDto)
    {
        var device = await GetAsync(newDeviceDto.Id);

        _imageService.DeleteImageAsync(device.ImagePath!);

        newDeviceDto.ImagePath = await _imageService
            .SaveImageAsync(newDeviceDto.ImageFile, DeviceConstants.SubFolder);

        return await base.Update(newDeviceDto);
    }

    public override async Task<DeviceDto> Delete(int id)
    {
        var device = await GetAsync(id);

        _imageService.DeleteImageAsync(device.ImagePath!);

        return await base.Delete(id);
    }
}
