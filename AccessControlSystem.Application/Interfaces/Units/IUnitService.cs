using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Units;

namespace AccessControlSystem.Application.Interfaces.Units;

public interface IUnitService : IBaseService<Unit, UnitDto, int>
{
}
