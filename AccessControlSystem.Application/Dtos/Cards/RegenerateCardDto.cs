namespace AccessControlSystem.Application.Dtos.Cards;

public class RegenerateCardDto
{
    public int UserId { get; set; }
    public string Mobile { get; set; } = default!;
    public string Email { get; set; } = default!;
}
