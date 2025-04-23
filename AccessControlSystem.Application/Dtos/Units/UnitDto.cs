using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Units;

public class UnitDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public int UserId { get; set; }
    public int SubscriptionId { get; set; }
}
