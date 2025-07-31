using AccessControlSystem.Application.Dtos.AccessGroupUnits;
using AccessControlSystem.Application.Interfaces.AccessGroupUnits;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.AccessGroupUnits;

[Route("api/[controller]")]
[ApiController]
public class AccessGroupUnitsController(IAccessGroupUnitService service) :
        BaseController<
            IAccessGroupUnitService,
            AccessGroupUnit,
            AccessGroupUnitDto,
            (int, int)>(service)
{
    [HttpGet("Get")]
    public async Task<IActionResult> Get(int accessGroupId, int unitId)
    {
        var id = (accessGroupId, unitId);

        return await base.Get(id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public override async Task<IActionResult> Get((int, int) id)
    {
        return await base.Get(id);
    }
}

