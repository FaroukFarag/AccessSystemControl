using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Units;

public class UnitRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Unit> specificationCombiner) :
    BaseRepository<Unit, int>(context, specificationCombiner), IUnitRepository
{
}
