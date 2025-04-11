using AccessControlSystem.Application.Dtos.AccessGroupDevices;
using AccessControlSystem.Application.Interfaces.AccessGroupDevices;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.AccessGroupDevices
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessGroupDevicesController(IAccessGroupDeviceService service) :
        BaseController<IAccessGroupDeviceService, AccessGroupDevice, AccessGroupDeviceDto, (int, int)>(service)
    {
        IAccessGroupDeviceService _service = service;

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
}
