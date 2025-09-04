using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Cards;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Application.Interfaces.Users;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Constants.Units;
using AccessControlSystem.Domain.Enums.Roles;
using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Units;

public class UnitService(
    IUnitRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IOrderingService<Unit> orderingService,
    IUserService userService,
    ICardService cardService) : BaseService<
        CreateUnitDto, UnitDto, UnitDto, UpdateUnitDto, Unit, int>(
        repository, unitOfWork, mapper), IUnitService
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IOrderingService<Unit> _orderingService = orderingService;
    private readonly IUserService _userService = userService;
    private readonly ICardService _cardService = cardService;
    private static readonly Dictionary<string, Action<BaseSpecification<Unit>>> OrderingRules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = spec => spec.OrderBy = s => s.Name,
        ["recent"] = spec => spec.OrderByDescending = s => s.CreatedAt,
    };

    public override async Task<ResultDto<CreateUnitDto>> CreateAsync(CreateUnitDto unitDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Unit",
            action: async () =>
            {
                unitDto.ImagePath = await _imageService.SaveImageAsync(
                    unitDto.ImageFile,
                    UnitConstants.SubFolder) ?? throw new InvalidOperationException("Image upload failed");

                return (await base.CreateAsync(unitDto)).ResultData
                    ?? throw new InvalidOperationException("Unit creation failed");
            });
    }

    public async Task<ResultDto<UnitDto>> GetWithIncludesAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Unit with Includes",
            action: async () =>
            {
                var unit = await _repository.GetAsync(id, new BaseSpecification<Unit>()
                {
                    Includes = [u => u.Cards],
                    IncludeChains =
                    [
                        new IncludeChain<Unit>
                        {
                            InitialInclude = u => u.AccessGroupUnits,
                            ThenIncludes = [
                                agu => (agu as AccessGroupUnit)!.AccessGroup,
                                ag => (ag as AccessGroup)!.AccessGroupDevices,
                                agd => (agd as AccessGroupDevice)!.Device,
                            ]
                        }
                    ]
                });
                return _mapper.Map<UnitDto>(unit);
            });
    }

    public override async Task<ResultDto<IEnumerable<UnitDto>>> GetAllAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Unit",
        action: async () =>
        {
            var unit = await _repository.GetAllAsync(new BaseSpecification<Unit>()
            {
                Includes = [u => u.Subscription]
            });

            return _mapper.Map<IEnumerable<UnitDto>>(unit);
        });
    }

    public async Task<ResultDto<IEnumerable<UnitDto>>> GetAllAsync(string orderBy = "Recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Units",
            action: async () =>
            {
                var specification = new BaseSpecification<Unit>();

                _orderingService.ApplyOrdering(specification, OrderingRules, orderBy);

                var units = await _repository.GetAllAsync(specification);

                return _mapper.Map<IEnumerable<UnitDto>>(units);
            });
    }

    public async Task<ResultDto<long>> GetUnitsCountAsync(bool isLastMonth)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Units Count",
            action: async () =>
            {
                if (!isLastMonth)
                    return await _repository.GetCountAsync();

                BaseSpecification<Unit> specification = new()
                {
                    Criteria = d => d.CreatedAt.Month < DateTime.Now.Month
                };

                return await _repository.GetCountAsync(specification);
            });
    }

    public override async Task<ResultDto<UpdateUnitDto>> UpdateAsync(UpdateUnitDto newUnitDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Unit",
            action: async () =>
            {
                var existingImagePath = (await base.GetAsync(newUnitDto.Id)).ResultData?.ImagePath;

                newUnitDto.ImagePath = await _imageService.SaveImageAsync(
                    newUnitDto.ImageFile,
                    UnitConstants.SubFolder) ?? throw new InvalidOperationException("Image upload failed");

                var updateResult = await base.UpdateAsync(newUnitDto);

                if (!string.IsNullOrEmpty(existingImagePath))
                {
                    _imageService.DeleteImage(existingImagePath);
                }

                return updateResult.ResultData
                    ?? throw new InvalidOperationException("Unit update failed");
            });
    }

    public async Task<ResultDto<UpdateUnitDto>> AssignOwnerToUnit(AssignOwnerToUnitDto assignOwnerToUnitDto)
    {
        ArgumentNullException.ThrowIfNull(assignOwnerToUnitDto);

        return await ExecuteServiceCallAsync(
            operationName: "Assign Owner to Unit",
            action: async () =>
            {
                var unit = await _repository.GetAsync(assignOwnerToUnitDto.UnitId, new BaseSpecification<Unit>
                {
                    IncludeChains =
                    [
                        new IncludeChain<Unit>
                        {
                            InitialInclude = u => u.AccessGroupUnits,
                            ThenIncludes =
                            [
                                agu => (agu as AccessGroupUnit)!.AccessGroup
                            ]
                        }
                    ]
                }) ?? throw new InvalidOperationException("Unit not found");
                var ownerResult = await _userService.GetUserByRoleAsync(assignOwnerToUnitDto.OwnerId, (int)RoleNames.Owner);

                if (!ownerResult.Succeeded || ownerResult.ResultData == null)
                {
                    throw new InvalidOperationException("Owner not found");
                }

                var owner = ownerResult.ResultData;

                owner.UnitId = unit.Id;

                var updateOwnerResult = await _userService.UpdateAsync(owner);

                if (!updateOwnerResult.Succeeded || updateOwnerResult.ResultData == null)
                {
                    throw new InvalidOperationException("Owner assignment failed");
                }

                var unitDto = _mapper.Map<UnitDto>(unit);
                var updateUnitDto = _mapper.Map<UpdateUnitDto>(unit);
                var cardResult = await _cardService.CreateAsync(
                    _mapper.Map<CreateCardDto>((owner, assignOwnerToUnitDto, unitDto, "normal")));

                if (!cardResult.Succeeded || cardResult.ResultData == null)
                {
                    throw new InvalidOperationException("Owner assignment failed");
                }

                updateUnitDto.AssignedOwner = owner.UserName;

                return (await base.UpdateAsync(updateUnitDto)).ResultData
                    ?? throw new InvalidOperationException("Owner assignment failed");
            });
    }

    public override async Task<ResultDto<UnitDto>> DeleteAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Delete Unit",
            action: async () =>
            {
                var unit = await base.GetAsync(id);

                if (unit.ResultData?.ImagePath is not null)
                {
                    _imageService.DeleteImage(unit.ResultData.ImagePath);
                }

                return (await base.DeleteAsync(id)).ResultData
                    ?? throw new InvalidOperationException("Unit deletion failed");
            });
    }
}
