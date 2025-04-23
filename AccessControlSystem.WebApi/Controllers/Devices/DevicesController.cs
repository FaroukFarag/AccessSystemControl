using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Devices;

[Route("api/[controller]")]
[ApiController]
public class DevicesController(IDeviceService service) :
    BaseController<IDeviceService, Device, DeviceDto, int>(service)
{
    [HttpPost("Create")]
    public override Task<IActionResult> Create([FromForm] DeviceDto dto)
    {
        return base.Create(dto);
    }

    [HttpPut("Update")]
    public override Task<IActionResult> Update([FromForm] DeviceDto dto)
    {
        return base.Update(dto);
    }
}
