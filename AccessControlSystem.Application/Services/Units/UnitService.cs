using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Units;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Units;

public class UnitService(
    IUnitRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<Unit, UnitDto, int>(repository, unitOfWork, mapper),
    IUnitService
{
}
