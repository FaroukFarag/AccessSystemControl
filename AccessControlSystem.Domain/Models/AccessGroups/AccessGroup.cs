using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;

namespace AccessControlSystem.Domain.Models.AccessGroups;

public class AccessGroup : BaseModel<int>
{
    public string Name { get; set; } = default!;

    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
}
