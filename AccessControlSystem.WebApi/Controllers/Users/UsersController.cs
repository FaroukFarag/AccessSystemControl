using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Users;
using AccessControlSystem.Domain.Enums.Roles;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControlSystem.WebApi.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService)
    : BaseController<IUserService, UserDto, UserDto, UserDto, UserDto, User,
        int>(userService)
{
    private readonly IUserService _userService = userService;

    [HttpGet("GetOwnerDetails")]
    public async Task<IActionResult> GetOwnerDetails(int id)
        => Ok(await _userService.GetUserByRoleAsync(id, (int)RoleNames.Owner));

    [HttpGet("GetAllOwners")]
    public async Task<IActionResult> GetAllOwners()
        => Ok(await _userService.GetAllUsersByRoleAsync((int)RoleNames.Owner));

    [HttpGet("GetUnassignedOwners")]
    public async Task<IActionResult> GetUnassignedOwners()
        => Ok(await _userService.GetUnassignedOwnersAsync());

    [HttpGet("GetAllOwners/{orderBy}")]
    public async Task<IActionResult> GetAll(string orderBy) => Ok(await _userService.GetAllUsersByRoleAsync((int)RoleNames.Owner, orderBy));

    [HttpGet("GetSubscriptionAdminDetails")]
    public async Task<IActionResult> GetSubscriptionAdminDetails(int id)
        => Ok(await _userService.GetUserByRoleAsync(id, (int)RoleNames.Owner));

    [HttpGet("GetAllSubscriptionAdmins")]
    public async Task<IActionResult> GetAllSubscriptionAdmins()
        => Ok(await _userService.GetAllUsersByRoleAsync((int)RoleNames.SubscriptionAdmin));

    [HttpGet("GetAllSubscriptionAdmins/{orderBy}")]
    public async Task<IActionResult> GetAllSubscriptionAdmins(string orderBy) => Ok(await _userService.GetAllUsersByRoleAsync((int)RoleNames.SubscriptionAdmin, orderBy));

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto loginDto)
        => await HandleLoginAsync(loginDto);

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var result = await _userService.ResetPasswordAsync(resetPasswordDto);

        return HandleResult(result.Succeeded, "Password reset successfully.", "Failed to reset password. User not found or invalid request.");
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
    {
        var result = await _userService.ForgotPasswordAsync(request);

        return HandleResult(result.Succeeded, "Password reset successfully.", "Failed to reset password. User not found or invalid request.");
    }

    private async Task<IActionResult> HandleLoginAsync(LoginDto loginDto)
    {
        var loggedInDto = await _userService.LoginAsync(loginDto);

        return loggedInDto is null
            ? Unauthorized(new { Message = "Invalid login attempt" })
            : Ok(loggedInDto);
    }

    private IActionResult HandleResult(bool result, string successMessage, string errorMessage)
    {
        return result
            ? Ok(new { Message = successMessage })
            : BadRequest(new { Message = errorMessage });
    }
}
