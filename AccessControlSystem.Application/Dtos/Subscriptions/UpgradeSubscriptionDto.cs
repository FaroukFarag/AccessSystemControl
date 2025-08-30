using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Enums.Subscriptions;

namespace AccessControlSystem.Application.Dtos.Subscriptions;

public class UpgradeSubscriptionDto : BaseModelDto<int>
{
    public SubscriptionType SubscriptionType { get; set; }
    public int MonthNumber { get; set; }
    public DateOnly StartDate { get; set; }
}
