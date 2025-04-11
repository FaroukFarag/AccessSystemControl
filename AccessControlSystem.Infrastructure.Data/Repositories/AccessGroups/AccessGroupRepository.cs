using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroups;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.AccessGroups;

public class AccessGroupRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<AccessGroup> specificationCombiner) :
    BaseRepository<AccessGroup, int>(context, specificationCombiner),
    IAccessGroupRepository
{
}
