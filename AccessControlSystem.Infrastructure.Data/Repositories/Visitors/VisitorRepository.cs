using AccessControlSystem.Domain.Interfaces.Repositories;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Visitors;

public class VisitorRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Visitor> specificationCombiner,
    IQueryBuilder<Visitor> queryBuilder,
    IEntityFinder<Visitor, int> entityFinder,
    IPaginationService paginationService,
    ILogger<VisitorRepository> logger) :
    BaseRepository<Visitor, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), IVisitorRepository
{
}
