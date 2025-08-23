using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Cards;

public class CardDto : BaseModelDto<int>
{
    public int OwnerId { get; set; }
    public int AirfobUserId { get; set; }
    public string UserName { get; set; } = default!;
    public int SiteId { get; set; }
}
