using AccessControlSystem.Domain.Interfaces.Repositories.Cards;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Cards;

public class CardRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Card> specificationCombiner) :
    BaseRepository<Card, int>(context, specificationCombiner), ICardRepository
{
}
