using AccessControlSystem.Domain.Interfaces.Repositories.Roles;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Roles;

public class RoleRepository(
AccessControlDbContext context,
    ISpecificationCombiner<Role> specificationCombiner) :
    BaseRepository<Role, int>(context, specificationCombiner), IRoleRepository
{
}
