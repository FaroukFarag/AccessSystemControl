using AccessControlSystem.Domain.Models.Abstraction;

namespace AccessControlSystem.Domain.Models.Cards;

public class Card : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public bool Active { get; set; }
}
