using AccessControlSystem.Domain.Interfaces.Repositories;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Visitors;
using AccessControlSystem.Infrastructure.Data.Context;
using AccessControlSystem.Infrastructure.Data.Repositories.Abstraction;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Visitors;

public class VisitorRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Visitor> specificationCombiner) :
    BaseRepository<Visitor, int>(context, specificationCombiner), IVisitorRepository
{
}
