using AccessControlSystem.Domain.Models.Subscriptions;
using Microsoft.AspNetCore.Identity;

namespace AccessControlSystem.Domain.Models.Users;

public class User : IdentityUser<int>
{
    public int? SubscriptionId { get; set; }

    public Subscription? Subscription { get; set; }
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = default!;
}
