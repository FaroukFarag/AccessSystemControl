using AccessControlSystem.Application.Common.Mappings;
using AccessControlSystem.Application.Common.Utilities;
using AccessControlSystem.Application.Configurations;
using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Constants.Devices;
using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Specifications.Absraction;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Devices;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.EventLogs;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AccessControlSystem.Application.Services.Devices;

public class DeviceService(
    IDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IAirfobDeviceService airfobDeviceService,
    IAirfobEventLogService airfobEventLogService,
    IOptions<ImageSettings> settings,
    IOrderingService<Device> orderingService) : BaseService<
        DeviceDto, DeviceDto, DeviceDto, DeviceDto, Device, int>(
        repository, unitOfWork, mapper), IDeviceService
{
    private readonly IDeviceRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IAirfobDeviceService _airfobDeviceService = airfobDeviceService;
    private readonly IAirfobEventLogService _airfobEventLogService = airfobEventLogService;
    private readonly ImageSettings _settings = settings.Value;
    private readonly IOrderingService<Device> _orderingService = orderingService;

    private static readonly Dictionary<string, Action<BaseSpecification<Device>>> OrderingRules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = spec => spec.OrderBy = s => s.Name,
        ["recent"] = spec => spec.OrderByDescending = s => s.CreatedAt,
        ["serial"] = spec => spec.OrderBy = s => s.Serial,
        ["macaddress"] = spec => spec.OrderBy = s => s.MacAddress,
    };

    public override async Task<ResultDto<DeviceDto>> CreateAsync(DeviceDto deviceDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Device",
            action: async () =>
            {
                ValidateDeviceDto(deviceDto);

                deviceDto.ImagePath = await SaveDeviceImageAsync(deviceDto.ImageFile!);

                await CreateDeviceInExternalSystemAsync(deviceDto);

                return await CreateDeviceInDatabaseAsync(deviceDto);
            });
    }

    public async Task<ResultDto<IEnumerable<DeviceDto>>> GetAllAsync(string orderBy = "Recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Devices",
            action: async () =>
            {
                var specification = new BaseSpecification<Device>();

                ApplyOrdering(specification, orderBy);

                var devices = await _repository.GetAllAsync(specification);

                return _mapper.Map<IEnumerable<DeviceDto>>(devices);
            });
    }

    public async Task<ResultDto<IEnumerable<DeviceDto>>> GetAvailableDevicesForAccessGroupAsync(int accessGroupId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Available Devices for Access Group",
            action: async () =>
            {
                var spec = CreateAvailableDevicesSpecification(accessGroupId);
                var devices = await _repository.GetAllAsync(spec);

                return _mapper.Map<IEnumerable<DeviceDto>>(devices);
            });
    }

    public async Task<ResultDto<IEnumerable<DeviceTrafficDto>>> GetDevicesTrafficAsync(int subscriptionId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Devices Traffic",
            action: async () =>
            {
                var eventLogs = await GetEventLogsFromExternalSystemAsync();
                var devices = await GetDevicesForSubscriptionAsync(subscriptionId, eventLogs);

                return MapDeviceTrafficDtos(eventLogs, devices);
            });
    }

    public async Task<ResultDto<IEnumerable<SubscriptionDeviceDto>>> GetSubscriptionDevicesAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Subscription Devices",
            action: async () =>
            {
                return await _repository.GetAllAsync(
                    d => new SubscriptionDeviceDto
                    {
                        DeviceName = d.Name,
                        StartDate = d.Subscription.StartDate,
                        EndDate = d.Subscription.EndDate,
                        RemainingPeriod = RenewalCalculator.GetRenewalInfo(d.Subscription.EndDate, false)
                    });
            });
    }

    public async Task<ResultDto<long>> GetDevicesCountAsync(bool isLastMonth = false)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Devices Count",
            action: async () =>
            {
                return isLastMonth
                    ? await GetLastMonthDevicesCountAsync()
                    : await GetTotalDevicesCountAsync();
            });
    }

    public override async Task<ResultDto<DeviceDto>> UpdateAsync(DeviceDto deviceDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Device",
            action: async () =>
            {
                ValidateDeviceDto(deviceDto);

                var existingDevice = await GetExistingDeviceAsync(deviceDto.Id);

                await UpdateDeviceImageAsync(existingDevice, deviceDto);

                return (await base.UpdateAsync(deviceDto)).ResultData;
            });
    }

    public override async Task<ResultDto<DeviceDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Device",
            action: async () =>
            {
                var device = await GetExistingDeviceAsync(id);

                DeleteDeviceImage(device);

                return (await base.DeleteAsync(id)).ResultData;
            });
    }

    private static void ValidateDeviceDto(DeviceDto deviceDto)
    {
        if (deviceDto.ImageFile == null)
        {
            throw new InvalidOperationException("Device image is required");
        }
    }

    private async Task<string> SaveDeviceImageAsync(IFormFile imageFile)
    {
        return await _imageService.SaveImageAsync(imageFile, DeviceConstants.SubFolder);
    }

    private void DeleteDeviceImage(DeviceDto device)
    {
        if (!string.IsNullOrEmpty(device.ImagePath))
        {
            _imageService.DeleteImage(device.ImagePath);
        }
    }

    private async Task UpdateDeviceImageAsync(DeviceDto existingDevice, DeviceDto newDeviceDto)
    {
        DeleteDeviceImage(existingDevice);

        newDeviceDto.ImagePath = await SaveDeviceImageAsync(newDeviceDto.ImageFile!);
    }

    private async Task CreateDeviceInExternalSystemAsync(DeviceDto deviceDto)
    {
        var airfobRequest = new CreateDevicesRequest
        {
            Devices = [_mapper.Map<CreateDeviceRequest>(deviceDto)]
        };

        var airfobResponse = await _airfobDeviceService.CreateDevicesAsync(airfobRequest);

        if (!airfobResponse.Succeeded)
        {
            _imageService.DeleteImage(deviceDto.ImagePath!);

            throw new InvalidOperationException("Failed to create device in external system");
        }
    }

    private async Task<DeviceDto> CreateDeviceInDatabaseAsync(DeviceDto deviceDto)
    {
        var createResult = await base.CreateAsync(deviceDto);

        if (!createResult.Succeeded || createResult.ResultData == null)
        {
            _imageService.DeleteImage(deviceDto.ImagePath!);

            throw new InvalidOperationException("Failed to create device in database");
        }

        return createResult.ResultData;
    }

    private async Task<DeviceDto> GetExistingDeviceAsync(int id)
    {
        var deviceResult = await base.GetAsync(id);

        if (!deviceResult.Succeeded || deviceResult.ResultData == null)
        {
            throw new InvalidOperationException($"Device with ID {id} not found");
        }

        return deviceResult.ResultData;
    }

    private BaseSpecification<Device> CreateAvailableDevicesSpecification(int accessGroupId)
    {
        return new BaseSpecification<Device>
        {
            Criteria = d => !d.AccessGroupDevices.Any(agd => agd.AccessGroupId == accessGroupId)
        };
    }

    private async Task<IEnumerable<Device>> GetDevicesForSubscriptionAsync(int subscriptionId, IEnumerable<GetEventLogResponse> eventLogs)
    {
        var airfobDeviceSerials = eventLogs.Select(l => l.DeviceSerial).ToHashSet();

        var spec = new BaseSpecification<Device>
        {
            Criteria = d => airfobDeviceSerials.Contains(d.Serial.ToString()) &&
                           d.SubscriptionId == subscriptionId
        };

        return await _repository.GetAllAsync(spec);
    }

    private async Task<IEnumerable<GetEventLogResponse>> GetEventLogsFromExternalSystemAsync()
    {
        var eventLogsResponse = await _airfobEventLogService.GetEventLogsAsync();

        if (!eventLogsResponse.Succeeded || eventLogsResponse.ResultData == null)
        {
            throw new InvalidOperationException("Failed to get event logs from external system");
        }

        return eventLogsResponse.ResultData.EventLogs;
    }

    private IEnumerable<DeviceTrafficDto> MapDeviceTrafficDtos(IEnumerable<GetEventLogResponse> eventLogs, IEnumerable<Device> devices)
    {
        return from log in eventLogs
               join device in devices
               on log.DeviceSerial equals device.Serial.ToString()
               select new DeviceTrafficDto
               {
                   TrafficType = DeviceTrafficCodeMapper.GetTrafficTypeDescription(log.Code),
                   Time = TimeOnly.FromDateTime(log.DateTime),
                   Date = DateOnly.FromDateTime(log.DateTime),
                   MacAddress = device.MacAddress,
                   ImagePath = GetFullImageUrl(device.ImagePath)
               };
    }

    private string GetFullImageUrl(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return string.Empty;
        }

        var normalizedPath = imagePath.Replace("\\", "/").TrimStart('/');
        return $"{_settings.BaseUrl.TrimEnd('/')}/{normalizedPath}";
    }

    private async Task<long> GetTotalDevicesCountAsync()
    {
        return await _repository.GetCountAsync();
    }

    private async Task<long> GetLastMonthDevicesCountAsync()
    {
        var lastMonth = DateTime.Now.AddMonths(-1);
        var specification = new BaseSpecification<Device>
        {
            Criteria = d => d.CreatedAt.Month == lastMonth.Month &&
                           d.CreatedAt.Year == lastMonth.Year
        };

        return await _repository.GetCountAsync(specification);
    }

    private void ApplyOrdering(BaseSpecification<Device> specification, string orderBy)
    {
        _orderingService.ApplyOrdering(specification, OrderingRules, orderBy);
    }
}
