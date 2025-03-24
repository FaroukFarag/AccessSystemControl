using AccessControlSystem.Application.Dtos.Roles;
using AccessControlSystem.Application.Interfaces.Roles;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Roles;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RolesController(IRoleService roleService) :
    BaseController<IRoleService, Role, RoleDto, int>(roleService)
{
}

