using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Cards;

public class GetUnitCardDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int UserId { get; set; }
    public string Status { get; set; } = default!;
}
