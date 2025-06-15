using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Airfob.Users;

[Route("api/[controller]")]
[ApiController]
public class AirfobUsersController(IAirfobUserService service) : ControllerBase
{
    private readonly IAirfobUserService _service = service;

    [HttpPost("CreateUsers")]
    public virtual async Task<IActionResult> CreateUsers(CreateUsersRequest request)
    {
        return Ok(await _service.CreateUsersAsync(request));
    }

    [HttpPost("CreateUserGroups")]
    public virtual async Task<IActionResult> UserGroups(CreateUserGroupsRequest request)
    {
        return Ok(await _service.CreateUserGroupsAsync(request));
    }

    [HttpGet("GetUsers")]
    public virtual async Task<IActionResult> GetUsers()
    {
        return Ok(await _service.GetUsersAsync());
    }

    [HttpGet("GetUserGroups")]
    public virtual async Task<IActionResult> GetUserGroups()
    {
        return Ok(await _service.GetUserGroupsAsync());
    }

    [HttpPost("AssignUserGroupMembers")]
    public virtual async Task<IActionResult> AssignUserGroupMembers(AssignUserGroupMembersRequest request)
    {
        return Ok(await _service.AssignUserGroupMembersAsync(request));
    }
}
