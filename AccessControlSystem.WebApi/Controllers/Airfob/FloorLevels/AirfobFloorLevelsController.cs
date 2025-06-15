using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.FloorLevels;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Airfob.FloorLevels;

[Route("api/[controller]")]
[ApiController]
public class AirfobFloorLevelsController(IAirfobFloorLevelService service) : ControllerBase
{
    private readonly IAirfobFloorLevelService _service = service;

    [HttpPost("Create")]
    public virtual async Task<IActionResult> Create(CreateFloorLevelsRequest request)
    {
        return Ok(await _service.CreateFloorLevelsAsync(request));
    }

    [HttpGet("GetAll")]
    public virtual async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetFloorLevelsAsync());
    }
}
