using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Units;

namespace AccessControlSystem.Application.Interfaces.Units;

public interface IUnitService : IBaseService<Unit, UnitDto, int>
{
    Task<ResultDto<UnitDto>> GetWithIncludesAsync(int id);
    Task<ResultDto<IEnumerable<UnitDto>>> GetAllAsync(string orderBy);
    Task<ResultDto<UnitDto>> AssignOwnerToUnit(AssignOwnerToUnitDto assignOwnerToUnitDto);
}
