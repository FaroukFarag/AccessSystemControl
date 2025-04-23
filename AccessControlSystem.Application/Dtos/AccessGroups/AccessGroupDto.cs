using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.AccessGroups;

public class AccessGroupDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
}
