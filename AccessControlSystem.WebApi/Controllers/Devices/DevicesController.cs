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
    private readonly IDeviceService _service = service;

    [HttpPost("Create")]
    public override Task<IActionResult> Create([FromForm] DeviceDto dto)
    {
        return base.Create(dto);
    }

    [HttpGet("GetAvailableDevicesForSubscription")]
    public async Task<IActionResult> GetAvailableDevicesForSubscription(int subscriptionId)
    {
        return Ok(await _service.GetAvailableDevicesForSubscription(subscriptionId));
    }

    [HttpGet("GetAvailableDevicesForAccessGroup")]
    public async Task<IActionResult> GetAvailableDevicesForAccessGroup(int accessGroupId)
    {
        return Ok(await _service.GetAvailableDevicesForAccessGroup(accessGroupId));
    }

    [HttpPut("Update")]
    public override Task<IActionResult> Update([FromForm] DeviceDto dto)
    {
        return base.Update(dto);
    }
}
