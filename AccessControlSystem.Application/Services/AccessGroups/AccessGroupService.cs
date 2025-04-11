using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Interfaces.AccessGroups;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroups;
using AutoMapper;

namespace AccessControlSystem.Application.Services.AccessGroups;

public class AccessGroupService(
    IAccessGroupRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<AccessGroup, AccessGroupDto, int>(repository, unitOfWork, mapper),
    IAccessGroupService
{
}
