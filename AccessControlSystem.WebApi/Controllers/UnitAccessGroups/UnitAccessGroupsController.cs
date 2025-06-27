using AccessControlSystem.Application.Dtos.UnitAccessGroups;
using AccessControlSystem.Application.Interfaces.UnitAccessGroups;
using AccessControlSystem.Domain.Models.UnitAccessGroups;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.UnitAccessGroups;

[Route("api/[controller]")]
[ApiController]
public class UnitAccessGroupsController(IUnitAccessGroupService service) :
BaseController<
            IUnitAccessGroupService,
            UnitAccessGroup,
            UnitAccessGroupDto,
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
