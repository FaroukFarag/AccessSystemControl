using AccessControlSystem.Common.Interfaces.Subscriptions;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Models.Units;
using Microsoft.AspNetCore.Identity;

namespace AccessControlSystem.Domain.Models.Users;

public class User : IdentityUser<int>, ISubscriptionEntity, IAuditable
{
    public int SubscriptionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Subscription Subscription { get; set; } = default!;
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = default!;
    public virtual ICollection<Unit>? Units { get; set; }
    public virtual ICollection<AccessGroup>? AccessGroups { get; set; }
}
