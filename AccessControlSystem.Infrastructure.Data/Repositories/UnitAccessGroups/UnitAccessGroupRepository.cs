using AccessControlSystem.Domain.Interfaces.Repositories.UnitAccessGroups;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.UnitAccessGroups;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.UnitAccessGroups;

public class UnitAccessGroupRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<UnitAccessGroup> specificationCombiner) :
    BaseRepository<UnitAccessGroup, (int AccessGroupId, int UnitId)>(
        context,
        specificationCombiner),
    IUnitAccessGroupRepository
{
}
