using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Users;

public class UserDto : BaseModelDto<int>
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Password { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string RoleId { get; set; }
}
