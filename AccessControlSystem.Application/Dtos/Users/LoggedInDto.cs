using AccessControlSystem.Application.Dtos.Units;

namespace AccessControlSystem.Application.Dtos.Users;

public class LoggedInDto
{
    public int? RoleId { get; set; }
    public int? SubscriptionId { get; set; }

    public string Token { get; set; } = default!;

    public IEnumerable<UnitDto> Units { get; set; } = default!;
}
