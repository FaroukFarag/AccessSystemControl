using AccessControlSystem.Application.Dtos.SubscriptionsDevices;
using AccessControlSystem.Application.Interfaces.SubscriptionsDevices;
using AccessControlSystem.Domain.Models.SubscriptionsDevices;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.SubscriptionsDevices;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsDevicesController(ISubscriptionDeviceService service) :
        BaseController<
            ISubscriptionDeviceService,
            SubscriptionDevice,
            SubscriptionDeviceDto,
            (int, int)>(service)
{
    [HttpGet("Get")]
    public async Task<IActionResult> Get(int accessGroupId, int deviceId)
    {
        var id = (accessGroupId, deviceId);

        return await base.Get(id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public override async Task<IActionResult> Get((int, int) id)
    {
        return await base.Get(id);
    }
}
