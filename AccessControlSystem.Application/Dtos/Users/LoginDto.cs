namespace AccessControlSystem.Application.Dtos.Users;

public class LoginDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime StartDateTime { get; set; }
}
