using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Devices;

[Route("api/[controller]")]
[ApiController]
public class DevicesController(IDeviceService service) :
    BaseController<IDeviceService, DeviceDto, DeviceDto, DeviceDto, DeviceDto,
        Device, int>(service)
{
    private readonly IDeviceService _service = service;

    [HttpGet("GetAll/{orderBy}")]
    public async Task<IActionResult> GetAll(string orderBy)
    {
        return Ok(await _service.GetAllAsync(orderBy));
    }

    [HttpGet("GetAvailableDevicesForAccessGroup")]
    public async Task<IActionResult> GetAvailableDevicesForAccessGroup(int accessGroupId)
    {
        return Ok(await _service.GetAvailableDevicesForAccessGroupAsync(accessGroupId));
    }

    [HttpGet("GetDevicesTraffic")]
    public async Task<IActionResult> GetDevicesTraffic()
    {
        return Ok(await _service.GetDevicesTrafficAsync());
    }

    [HttpGet("GetSubscriptionDevices")]
    public async Task<IActionResult> GetSubscriptionDevices()
    {
        return Ok(await _service.GetSubscriptionDevicesAsync());
    }

    [HttpGet("GetDevicesCount")]
    public async Task<IActionResult> GetDevicesCount()
    {
        return Ok(await _service.GetDevicesCountAsync());
    }

    [HttpGet("GetLastMonthDevicesCount")]
    public async Task<IActionResult> GetLastMonthDevicesCount()
    {
        return Ok(await _service.GetDevicesCountAsync(true));
    }

    [HttpPut("Update")]
    public override Task<IActionResult> Update([FromForm] DeviceDto dto)
    {
        return base.Update(dto);
    }
}
