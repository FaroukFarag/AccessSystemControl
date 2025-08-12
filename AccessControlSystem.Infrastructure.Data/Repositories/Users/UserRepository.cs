using AccessControlSystem.Domain.Interfaces.Repositories.Users;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Users;
public class UserRepository(AccessControlDbContext context,
    ISpecificationCombiner<User> specificationCombiner,
    IQueryBuilder<User> queryBuilder,
    IEntityFinder<User, int> entityFinder,
    IPaginationService paginationService,
    ILogger<UserRepository> logger) :
    BaseRepository<User, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), IUserRepository
{
}
