using AccessControlSystem.Application.Dtos.UnitAccessGroups;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.UnitAccessGroups;

namespace AccessControlSystem.Application.Interfaces.UnitAccessGroups;

public interface IUnitAccessGroupService :
    IBaseService<
        UnitAccessGroup,
        UnitAccessGroupDto,
        (int AccessGroupId, int UnitId)>
{
}
