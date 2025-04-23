using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Roles;

public class RoleDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
}
