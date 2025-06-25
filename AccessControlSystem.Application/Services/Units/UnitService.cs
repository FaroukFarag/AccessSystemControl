using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Domain.Constants.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Units;

public class UnitService(
    IUnitRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) :
    BaseService<Unit, UnitDto, int>(repository, unitOfWork, mapper),
    IUnitService
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;

    public override async Task<UnitDto> CreateAsync(UnitDto unitDto)
    {
        unitDto.ImagePath = await _imageService
            .SaveImageAsync(unitDto.ImageFile!, SubscriptionConstants.SubFolder);

        return await base.CreateAsync(unitDto);
    }

    public override async Task<UnitDto> GetAsync(int id)
    {
        var unit = await _repository.GetAsync(id, new BaseSpecification<Unit>
        {
            Includes = [u => u.Subscription, u => u.Owner!, u => u.AccessGroups],
            IncludeChains = [
                new IncludeChain<Unit>
                {
                    InitialInclude = u => u.AccessGroups!,
                    ThenIncludes = [
                        ag => (ag as AccessGroup)!.AccessGroupDevices,
                        agd => (agd as AccessGroupDevice)!.Device
                    ]
                }
            ]
        });

        return _mapper.Map<UnitDto>(unit);
    }

    public override async Task<UnitDto> UpdateAsync(UnitDto newUnitDto)
    {
        var unit = await GetAsync(newUnitDto.Id);

        _imageService.DeleteImageAsync(unit.ImagePath!);

        newUnitDto.ImagePath = await _imageService
            .SaveImageAsync(newUnitDto.ImageFile!, SubscriptionConstants.SubFolder);

        return await base.UpdateAsync(newUnitDto);
    }

    public override async Task<UnitDto> DeleteAsync(int id)
    {
        var unit = await GetAsync(id);

        _imageService.DeleteImageAsync(unit.ImagePath!);

        return await base.DeleteAsync(id);
    }

    public async Task<UnitDto> AssignOwnerToUnit(AssignOwnerToUnitDto assignOwnerToUnitDto)
    {
        var unit = await GetAsync(assignOwnerToUnitDto.UnitId);

        unit.OwnerId = assignOwnerToUnitDto.OwnerId;

        return await base.UpdateAsync(unit);
    }
}
