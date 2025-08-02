using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Application.Services.Shared;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Constants.Units;
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
    IImageService imageService) : BaseService<Unit, UnitDto, int>(repository, unitOfWork, mapper), IUnitService
{
    private readonly IUnitRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;

    private static readonly BaseSpecification<Unit> UnitWithIncludesSpec = new()
    {
        Includes = [u => u.Subscription, u => u.Owner!],
        IncludeChains =
        [
            new IncludeChain<Unit>
            {
                InitialInclude = u => u.AccessGroupUnits!,
                ThenIncludes = [
                    agu => (agu as AccessGroupUnit)!.AccessGroup,
                    ag => (ag as AccessGroup)!.AccessGroupDevices,
                    agd => (agd as AccessGroupDevice)!.Device
                ]
            }
        ]
    };

    public override async Task<ResultDto<UnitDto>> CreateAsync(UnitDto unitDto)
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


    public override async Task<ResultDto<IEnumerable<UnitDto>>> GetAllAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Unit",
        action: async () =>
        {
            var unit = await _repository.GetAllAsync(new BaseSpecification<Unit>()
            {
                Includes = [u => u.Subscription, u => u.Owner!]
            });

            return _mapper.Map<IEnumerable<UnitDto>>(unit);
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
                    Includes = [u => u.Owner!],
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

    public override async Task<ResultDto<UnitDto>> UpdateAsync(UnitDto newUnitDto)
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

    public async Task<ResultDto<UnitDto>> AssignOwnerToUnit(AssignOwnerToUnitDto assignOwnerToUnitDto)
    {
        ArgumentNullException.ThrowIfNull(assignOwnerToUnitDto);

        return await ExecuteServiceCallAsync(
            operationName: "Assign Owner to Unit",
            action: async () =>
            {
                var unitResult = await GetAsync(assignOwnerToUnitDto.UnitId);

                if (!unitResult.Succeeded || unitResult.ResultData == null)
                {
                    throw new InvalidOperationException("Unit not found");
                }

                var unitDto = unitResult.ResultData;

                unitDto.OwnerId = assignOwnerToUnitDto.OwnerId;

                return (await base.UpdateAsync(unitDto)).ResultData
                    ?? throw new InvalidOperationException("Owner assignment failed");
            });
    }
}
