using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Units;

[Route("api/[controller]")]
[ApiController]
public class UnitsController(IUnitService service) :
    BaseController<IUnitService, Unit, UnitDto, int>(service)
{
}
