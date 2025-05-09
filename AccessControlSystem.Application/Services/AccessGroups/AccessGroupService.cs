using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Interfaces.AccessGroups;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;

namespace AccessControlSystem.Application.Services.AccessGroups;

public class AccessGroupService(
    IAccessGroupRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<AccessGroup, AccessGroupDto, int>(repository, unitOfWork, mapper),
    IAccessGroupService
{
    private readonly IAccessGroupRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async override Task<AccessGroupDto> GetAsync(int id)
    {
        var accessGroup = await _repository.GetAsync(id, new BaseSpecification<AccessGroup>
        {
            Includes = [ag => ag.AccessGroupDevices],
            IncludesThen = [
                (ag => ag.AccessGroupDevices, sd => (sd as AccessGroupDevice)!.Device)
            ]
        });
        var subscriptionDto = _mapper.Map<AccessGroupDto>(accessGroup);

        return subscriptionDto;
    }
}
