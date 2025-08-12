using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Units;

public class UnitRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Unit> specificationCombiner,
    IQueryBuilder<Unit> queryBuilder,
    IEntityFinder<Unit, int> entityFinder,
    IPaginationService paginationService,
    ILogger<UnitRepository> logger) :
    BaseRepository<Unit, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), IUnitRepository
{
}
