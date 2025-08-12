namespace AccessControlSystem.Application.Dtos.Visitors;

public class CreateVisitorDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SiteId { get; set; }
    public string? InviteToken { get; set; }
    public int SubscriptionId { get; set; }

    public IEnumerable<int> AccessGroupIds { get; set; } = default!;
}
