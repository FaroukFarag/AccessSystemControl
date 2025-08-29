using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.AccessGroups;

namespace AccessControlSystem.Application.Dtos.Visitors;

public class VisitorDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SiteId { get; set; }
    public int AirfobUserId { get; set; }
    public string? InviteToken { get; set; }
    public int SubscriptionId { get; set; }

    public IEnumerable<AccessGroupDto> AccessGroups { get; set; } = default!;
}
