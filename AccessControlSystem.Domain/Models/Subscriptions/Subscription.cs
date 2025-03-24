using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Models.Abstraction;

namespace AccessControlSystem.Domain.Models.Subscriptions;

public class Subscription : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public SubscriptionPlan Plan { get; set; }
    public decimal PaymentPerMonth { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
