using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.AccessGroups;

public class GetAllAccessGroupsDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public int SubscriptionId { get; set; }
    public int AirfobAccessLevelId { get; set; }
    public long DevicesCount { get; set; }
}
