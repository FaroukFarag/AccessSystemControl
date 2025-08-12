using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Units;

[Route("api/[controller]")]
[ApiController]
public class UnitsController(IUnitService service) : BaseController<IUnitService,
    UnitDto, UnitDto, UnitDto, UnitDto, Unit, int>(service)
{
    private readonly IUnitService _service = service;

    public async override Task<IActionResult> Get(int id)
    {
        var unitDto = await _service.GetWithIncludesAsync(id);

        if (unitDto == null)
            return NotFound();

        return Ok(unitDto);
    }

    [HttpGet("GetAll/{orderBy}")]
    public async Task<IActionResult> GetAll(string orderBy)
    {
        return Ok(await _service.GetAllAsync(orderBy));
    }

    [HttpPut("AssignOwnerToUnit")]
    public virtual async Task<IActionResult> AssignOwnerToUnit(AssignOwnerToUnitDto assignOwnerToUnitDto)
    {
        return Ok(await _service.AssignOwnerToUnit(assignOwnerToUnitDto));
    }
}
