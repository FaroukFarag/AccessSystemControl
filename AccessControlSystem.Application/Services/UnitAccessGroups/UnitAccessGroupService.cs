using AccessControlSystem.Application.Dtos.UnitAccessGroups;
using AccessControlSystem.Application.Interfaces.UnitAccessGroups;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.UnitAccessGroups;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.UnitAccessGroups;
using AutoMapper;

namespace AccessControlSystem.Application.Services.UnitAccessGroups;

public class UnitAccessGroupService(
    IUnitAccessGroupRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        UnitAccessGroup,
        UnitAccessGroupDto,
        (int AccessGroupId, int UnitId)>(
        repository,
        unitOfWork,
        mapper),
    IUnitAccessGroupService
{
}
