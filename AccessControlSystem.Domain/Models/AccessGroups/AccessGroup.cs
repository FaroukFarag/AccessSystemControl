using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Shared;

namespace AccessControlSystem.Domain.Models.AccessGroups;

public class AccessGroup : SubscriptionEntity
{
    public string Name { get; set; } = default!;
    public int? AirfobAccessLevelId { get; set; }

    public IEnumerable<AccessGroupDevice> AccessGroupDevices { get; set; } = default!;
    public IEnumerable<AccessGroupUnit> AccessGroupUnits { get; set; } = default!;
}
