using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupUnits;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.AccessGroupUnits;

public class AccessGroupUnitRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<AccessGroupUnit> specificationCombiner) :
    BaseRepository<AccessGroupUnit, (int AccessGroupId, int UnitId)>(
        context,
        specificationCombiner),
    IAccessGroupUnitRepository
{
}
