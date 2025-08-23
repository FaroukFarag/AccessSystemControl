using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Interfaces.AccessGroups;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Specifications.Absraction;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.AccessLevels;
using AutoMapper;
using System.Linq.Expressions;

namespace AccessControlSystem.Application.Services.AccessGroups;

public class AccessGroupService(
    IAccessGroupRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAirfobAccessLevelService airfobAccessLevelService,
    IAirfobDeviceService airfobDeviceService,
    IOrderingService<AccessGroup> orderingService) : BaseService<AccessGroupDto, GetAllAccessGroupsDto,
    AccessGroupDto, AccessGroupDto, AccessGroup, int>(repository, unitOfWork, mapper), IAccessGroupService
{
    private readonly IAccessGroupRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IAirfobAccessLevelService _airfobAccessLevelService = airfobAccessLevelService;
    private readonly IAirfobDeviceService _airfobDeviceService = airfobDeviceService;
    private readonly IOrderingService<AccessGroup> _orderingService = orderingService;

    private static readonly BaseSpecification<AccessGroup> AccessGroupWithDevicesSpec = new()
    {
        IncludeChains =
        [
            new IncludeChain<AccessGroup>
            {
                InitialInclude = ag => ag.AccessGroupDevices,
                ThenIncludes = [agd => (agd as AccessGroupDevice)!.Device]
            }
        ]
    };
    private static readonly Dictionary<string, Action<BaseSpecification<AccessGroup>>> OrderingRules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = spec => spec.OrderBy = s => s.Name,
        ["recent"] = spec => spec.OrderByDescending = s => s.CreatedAt,
    };

    public override async Task<ResultDto<AccessGroupDto>> CreateAsync(AccessGroupDto accessGroupDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Access Group",
            action: async () => await CreateAccessGroupWithDependencies(accessGroupDto));
    }

    public override async Task<ResultDto<AccessGroupDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get {nameof(AccessGroup)} by ID",
            action: async () =>
            {
                var entity = await _repository.GetAsync(id, AccessGroupWithDevicesSpec);
                return _mapper.Map<AccessGroupDto>(entity);
            });
    }

    public override async Task<ResultDto<IEnumerable<GetAllAccessGroupsDto>>> GetAllAsync()
    {
        return await GetAllAsync(orderBy: "recent");
    }

    public async Task<ResultDto<IEnumerable<GetAllAccessGroupsDto>>> GetAllAsync(string orderBy = "recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Access Groups",
            action: async () =>
            {
                var specification = new BaseSpecification<AccessGroup>();

                _orderingService.ApplyOrdering(specification, OrderingRules, orderBy);

                return await _repository.GetAllAsync(BuildGetAllSelector(), specification);
            });
    }

    private async Task<AccessGroupDto> CreateAccessGroupWithDependencies(AccessGroupDto dto)
    {
        var accessLevels = await CreateAccessLevelsInExternalSystem(dto);

        dto.AirfobAccessLevelId = accessLevels.First().Id;

        var createResult = await base.CreateAsync(dto);

        if (!createResult.Succeeded)
            throw new InvalidOperationException("Failed to create access group in database");

        await AssignDevicesToAccessLevels(dto, accessLevels);

        return createResult.ResultData!;
    }

    private Expression<Func<AccessGroup, GetAllAccessGroupsDto>> BuildGetAllSelector() =>
        ag => new GetAllAccessGroupsDto
        {
            Id = ag.Id,
            Name = ag.Name,
            SubscriptionId = ag.SubscriptionId,
            AirfobAccessLevelId = ag.AirfobAccessLevelId,
            DevicesCount = ag.AccessGroupDevices.Count()
        };

    private async Task<IEnumerable<CreateAccessLevelResponse>> CreateAccessLevelsInExternalSystem(AccessGroupDto accessGroupDto)
    {
        var request = new CreateAccessLevelsRequest
        {
            Levels = [_mapper.Map<CreateAccessLevelRequest>(accessGroupDto)]
        };

        var response = await _airfobAccessLevelService.CreateAccessLevelsAsync(request);

        if (!response.Succeeded || response.ResultData == null)
        {
            throw new InvalidOperationException("Failed to create access levels in external system");
        }

        return response.ResultData;
    }

    private async Task AssignDevicesToAccessLevels(AccessGroupDto accessGroupDto, IEnumerable<CreateAccessLevelResponse> accessLevels)
    {
        var deviceIds = accessGroupDto.Devices!
            .Where(d => d.AirfobDeviceId.HasValue)
            .Select(d => d.AirfobDeviceId!.Value);

        if (!deviceIds.Any())
        {
            return;
        }

        var request = new AssignAccessLevelsByDevicesRequest
        {
            DeviceIds = deviceIds,
            AccessLevelIds = accessLevels.Select(al => al.Id)
        };

        var response = await _airfobDeviceService.AssignAccessLevelsByDevices(request);

        if (!response.Succeeded)
        {
            throw new Exception("Access group created but device assignment failed");
        }
    }
}
