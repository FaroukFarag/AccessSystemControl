using AccessControlSystem.Application.Dtos.AccessGroupUnits;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupUnits;

namespace AccessControlSystem.Application.Interfaces.AccessGroupUnits;

public interface IAccessGroupUnitService :
    IBaseService<
        AccessGroupUnit,
        AccessGroupUnitDto,
        (int AccessGroupId, int UnitId)>
{
}
