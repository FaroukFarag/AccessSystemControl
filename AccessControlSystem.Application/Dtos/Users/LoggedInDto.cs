namespace AccessControlSystem.Application.Dtos.Users;

public class LoggedInDto
{
    public int UserId { get; set; }

    public string Token { get; set; } = default!;
}
