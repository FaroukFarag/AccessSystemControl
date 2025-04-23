using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroups;

namespace AccessControlSystem.Application.Interfaces.AccessGroups;

public interface IAccessGroupService : IBaseService<AccessGroup, AccessGroupDto, int>
{
}
