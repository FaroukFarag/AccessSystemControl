using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.AccessGroups;

public class AccessGroupRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<AccessGroup> specificationCombiner,
    IQueryBuilder<AccessGroup> queryBuilder,
    IEntityFinder<AccessGroup, int> entityFinder,
    IPaginationService paginationService,
    ILogger<AccessGroupRepository> logger) :
    BaseRepository<AccessGroup, int>(context, specificationCombiner,
        queryBuilder, entityFinder, paginationService, logger),
    IAccessGroupRepository
{
}
