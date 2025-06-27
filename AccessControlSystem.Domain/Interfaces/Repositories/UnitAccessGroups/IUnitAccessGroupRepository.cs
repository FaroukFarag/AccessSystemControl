using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.UnitAccessGroups;

namespace AccessControlSystem.Domain.Interfaces.Repositories.UnitAccessGroups;

public interface IUnitAccessGroupRepository :
    IBaseRepository<UnitAccessGroup, (int AccessGroupId, int UnitId)>
{
}
