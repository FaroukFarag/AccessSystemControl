using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroupUnits;

namespace AccessControlSystem.Domain.Models.AccessGroups;

public class AccessGroup : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public int? AirfobAccessLevelId { get; set; }

    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
    public IEnumerable<AccessGroupUnit> AccessGroupUnits { get; set; } = default!;
}
