using AccessControlSystem.Application.Dtos.Abstraction;

namespace AccessControlSystem.Application.Dtos.Cards;

public class CardDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public bool Active { get; set; }
}
