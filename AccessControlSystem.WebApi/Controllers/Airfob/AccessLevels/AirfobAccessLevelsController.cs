using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.AccessLevels;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Airfob.AccessLevels;

[Route("api/[controller]")]
[ApiController]
public class AirfobAccessLevelsController(IAirfobAccessLevelService service) : ControllerBase
{
    private readonly IAirfobAccessLevelService _service = service;

    [HttpPost("Create")]
    public virtual async Task<IActionResult> Create(CreateAccessLevelsRequest request)
    {
        return Ok(await _service.CreateAccessLevelsAsync(request));
    }

    [HttpGet("GetAll")]
    public virtual async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAccessLevelsAsync());
    }
}
