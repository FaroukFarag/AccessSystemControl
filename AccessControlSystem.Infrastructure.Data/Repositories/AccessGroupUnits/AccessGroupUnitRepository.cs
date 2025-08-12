using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupUnits;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.AccessGroupUnits;

public class AccessGroupUnitRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<AccessGroupUnit> specificationCombiner,
    IQueryBuilder<AccessGroupUnit> queryBuilder,
    IEntityFinder<AccessGroupUnit, (int AccessGroupId, int UnitId)> entityFinder,
    IPaginationService paginationService,
    ILogger<AccessGroupUnitRepository> logger) :
    BaseRepository<AccessGroupUnit, (int AccessGroupId, int UnitId)>(
        context, specificationCombiner, queryBuilder, entityFinder,
        paginationService, logger),
    IAccessGroupUnitRepository
{
}
