using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Models.AccessGroups;

public class AccessGroup : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public int? AirfobAccessLevelId { get; set; }
    public int? OwnerId { get; set; }
    public int? UnitId { get; set; }

    public User? Owner { get; set; }
    public Unit? Unit { get; set; }
    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
}
