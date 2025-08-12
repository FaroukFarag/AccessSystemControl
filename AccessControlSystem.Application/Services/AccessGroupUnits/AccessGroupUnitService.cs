using AccessControlSystem.Application.Dtos.AccessGroupUnits;
using AccessControlSystem.Application.Interfaces.AccessGroupUnits;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupUnits;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AutoMapper;

namespace AccessControlSystem.Application.Services.AccessGroupUnits;

public class AccessGroupUnitService(
    IAccessGroupUnitRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        AccessGroupUnitDto,
        AccessGroupUnitDto,
        AccessGroupUnitDto,
        AccessGroupUnitDto,
        AccessGroupUnit,
        (int AccessGroupId, int UnitId)>(
        repository,
        unitOfWork,
        mapper),
    IAccessGroupUnitService
{
}
