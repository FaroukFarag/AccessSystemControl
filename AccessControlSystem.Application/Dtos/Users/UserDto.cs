using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Dtos.Subscriptions;
using AccessControlSystem.Application.Dtos.Units;

namespace AccessControlSystem.Application.Dtos.Users;

public class UserDto : BaseModelDto<int>
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Password { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public int RoleId { get; set; }
    public int? SubscriptionId { get; set; }

    public SubscriptionDto? Subscription { get; set; }
    public virtual ICollection<UnitDto>? Units { get; set; }
    public virtual ICollection<AccessGroupDto>? AccessGroups { get; set; }
}
