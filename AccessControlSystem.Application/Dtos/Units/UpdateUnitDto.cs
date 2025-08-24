using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Units;

public class UpdateUnitDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }
    public decimal Area { get; set; }
    public int CardNumber { get; set; }
    public string? AssignedOwner { get; set; }
    public int SubscriptionId { get; set; }
}
