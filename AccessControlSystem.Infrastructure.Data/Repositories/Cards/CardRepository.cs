using AccessControlSystem.Domain.Interfaces.Repositories.Cards;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Cards;

public class CardRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Card> specificationCombiner,
    IQueryBuilder<Card> queryBuilder,
    IEntityFinder<Card, int> entityFinder,
    IPaginationService paginationService,
    ILogger<CardRepository> logger) :
    BaseRepository<Card, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), ICardRepository
{
}
