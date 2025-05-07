using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.Cards;

namespace AccessControlSystem.Domain.Interfaces.Repositories.Cards;

public interface ICardRepository : IBaseRepository<Card, int>
{
}
