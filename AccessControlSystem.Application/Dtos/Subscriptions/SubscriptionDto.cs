using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Subscriptions;

public class SubscriptionDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
}
