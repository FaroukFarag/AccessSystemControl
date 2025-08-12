using AccessControlSystem.Domain.Interfaces.Repositories.Roles;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Roles;

public class RoleRepository(
AccessControlDbContext context,
    ISpecificationCombiner<Role> specificationCombiner,
    IQueryBuilder<Role> queryBuilder,
    IEntityFinder<Role, int> entityFinder,
    IPaginationService paginationService,
    ILogger<RoleRepository> logger) :
    BaseRepository<Role, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), IRoleRepository
{
}
