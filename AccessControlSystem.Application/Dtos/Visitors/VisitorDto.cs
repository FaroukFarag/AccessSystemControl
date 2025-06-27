using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Visitors;

public class VisitorDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public DateTime VisitDate { get; set; }
    public int UnitId { get; set; }
    public int SubscriptionId { get; set; }
}
