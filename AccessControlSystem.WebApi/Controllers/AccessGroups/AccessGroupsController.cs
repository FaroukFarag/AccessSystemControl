using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Interfaces.AccessGroups;
using AccessControlSystem.Domain.Models.AccessGroups;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.AccessGroups;

[Route("api/[controller]")]
[ApiController]
public class AccessGroupsController(IAccessGroupService service) :
    BaseController<IAccessGroupService, AccessGroup, AccessGroupDto, int>(service)
{
}
