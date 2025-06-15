using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Schedules;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Airfob.Schedules;

[Route("api/[controller]")]
[ApiController]
public class AirfobSchedulesController(IAirfobScheduleService service) : ControllerBase
{
    private readonly IAirfobScheduleService _service = service;

    [HttpPost("Create")]
    public virtual async Task<IActionResult> Create(CreateSchedulesRequest request)
    {
        return Ok(await _service.CreateSchedulesAsync(request));
    }

    [HttpGet("GetAll")]
    public virtual async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetSchedulesAsync());
    }
}
