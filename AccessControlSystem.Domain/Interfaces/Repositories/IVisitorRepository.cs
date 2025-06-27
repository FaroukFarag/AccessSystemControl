using AccessControlSystem.Domain.Interfaces.Repositories.Abstraction;
using AccessControlSystem.Domain.Models.Visitors;

namespace AccessControlSystem.Domain.Interfaces.Repositories;

public interface IVisitorRepository : IBaseRepository<Visitor, int>
{
}
