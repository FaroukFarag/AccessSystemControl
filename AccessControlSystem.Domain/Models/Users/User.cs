using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Units;
using Microsoft.AspNetCore.Identity;

namespace AccessControlSystem.Domain.Models.Users;

public class User : IdentityUser<int>
{
    public int? SubscriptionId { get; set; }

    public Subscription? Subscription { get; set; }
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = default!;
    public virtual ICollection<Unit>? Units { get; set; }
    public virtual ICollection<AccessGroup>? AccessGroups { get; set; }
}
