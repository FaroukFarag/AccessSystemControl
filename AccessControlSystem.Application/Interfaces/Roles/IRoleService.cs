using AccessControlSystem.Application.Dtos.Roles;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Roles;

namespace AccessControlSystem.Application.Interfaces.Roles;

public interface IRoleService : IBaseService<Role, RoleDto, int>
{
}
