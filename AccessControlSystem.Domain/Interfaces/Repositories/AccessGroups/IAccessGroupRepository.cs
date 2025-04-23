using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroups;

namespace AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;

public interface IAccessGroupRepository : IBaseRepository<AccessGroup, int>
{
}
