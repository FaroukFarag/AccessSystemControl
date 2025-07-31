using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupUnits;

namespace AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupUnits;

public interface IAccessGroupUnitRepository :
    IBaseRepository<AccessGroupUnit, (int AccessGroupId, int UnitId)>
{
}
