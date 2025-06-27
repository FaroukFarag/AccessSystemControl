using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Shared.Attributs;

namespace AccessControlSystem.Domain.Models.UnitAccessGroups;

public class UnitAccessGroup
{
    [CompositeKey]
    public int AccessGroupId { get; set; }
    [CompositeKey]
    public int UnitId { get; set; }

    public AccessGroup AccessGroup { get; set; } = default!;
    public Unit Unit { get; set; } = default!;
}
