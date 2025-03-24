using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.Roles;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Roles;

public interface IRoleRepository : IBaseRepository<Role, int>
{
}
