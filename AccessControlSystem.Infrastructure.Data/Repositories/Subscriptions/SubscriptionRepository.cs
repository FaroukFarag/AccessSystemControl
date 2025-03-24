using AccessControlSystem.Domain.Interfaces.Repositories.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Subscriptions;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Subscriptions;

public class SubscriptionRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Subscription> specificationCombiner) :
    BaseRepository<Subscription, int>(context, specificationCombiner), ISubscriptionRepository
{
}
