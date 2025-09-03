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
using AutoMapper;
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
    };

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

    public async Task<ResultDto<IEnumerable<DeviceDto>>> GetAllAsync(string orderBy = "Recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Devices",
            action: async () =>
            {
                var specification = new BaseSpecification<Device>();

                _orderingService.ApplyOrdering(specification, OrderingRules, orderBy);

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
                var spec = new BaseSpecification<Device>
                {
                    Criteria = d => !d.AccessGroupDevices.Any(agd => agd.AccessGroupId == accessGroupId)
                };

                var devices = await _repository.GetAllAsync(spec);

                return _mapper.Map<IEnumerable<DeviceDto>>(devices);
            });
    }

    public async Task<ResultDto<IEnumerable<DeviceTrafficDto>>> GetDevicesTrafficAsync(int subscriptionId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Available Devices for Access Group",
            action: async () =>
            {
                var eventLogsResponse = await _airfobEventLogService.GetEventLogsAsync();

                if (!eventLogsResponse.Succeeded && eventLogsResponse.ResultData is null)
                    throw new InvalidOperationException("Failed to get event logs");

                var airfobDevicesLog = eventLogsResponse.ResultData.EventLogs;
                var airfobDeviceSerials = airfobDevicesLog.Select(l => l.DeviceSerial).ToHashSet();
                var devices = await _repository.GetAllAsync(new BaseSpecification<Device>
                {
                    Criteria = d => airfobDeviceSerials.Contains(d.Serial) && d.SubscriptionId == subscriptionId
                });
                var trafficList = (from log in airfobDevicesLog
                                   join device in devices
                                   on log.DeviceSerial equals device.Serial.ToString()
                                   select new DeviceTrafficDto
                                   {
                                       TrafficType = DeviceTrafficCodeMapper.GetTrafficTypeDescription(log.Code),
                                       Time = TimeOnly.FromDateTime(log.DateTime),
                                       Date = DateOnly.FromDateTime(log.DateTime),
                                       MacAddress = device.MacAddress,
                                       ImagePath = $"{_settings.BaseUrl.TrimEnd('/')}/{device.ImagePath.Replace("\\", "/").TrimStart('/')}"
                                   });

                return trafficList;
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
                if (!isLastMonth)
                    return await _repository.GetCountAsync();

                BaseSpecification<Device> specification = new()
                {
                    Criteria = d => d.CreatedAt.Month < DateTime.Now.Month
                };

                return await _repository.GetCountAsync(specification);
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
