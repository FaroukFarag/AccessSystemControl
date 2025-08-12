using AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Subscriptions;

public class SubscriptionRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Subscription> specificationCombiner,
    IQueryBuilder<Subscription> queryBuilder,
    IEntityFinder<Subscription, int> entityFinder,
    IPaginationService paginationService,
    ILogger<SubscriptionRepository> logger) :
    BaseRepository<Subscription, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), ISubscriptionRepository
{
}
