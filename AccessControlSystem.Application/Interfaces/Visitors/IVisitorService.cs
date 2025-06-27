using AccessControlSystem.Application.Dtos.Visitors;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Visitors;

namespace AccessControlSystem.Application.Interfaces.Visitors;

public interface IVisitorService : IBaseService<Visitor, VisitorDto, int>
{
}
