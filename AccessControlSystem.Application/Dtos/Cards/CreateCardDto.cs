using AccessControlSystem.Application.Dtos.Units;

namespace AccessControlSystem.Application.Dtos.Cards;

public class CreateCardDto
{
    public int OwnerId { get; set; }
    public int SubscriptionId { get; set; }
    public int AirfobUserId { get; set; }
    public string UserName { get; set; } = default!;
    public int SiteId { get; set; }
    public string Email { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public UnitDto Unit { get; set; } = default!;
    public string Type { get; set; } = default!;
}
