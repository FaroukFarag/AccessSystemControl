﻿using AccessControlSystem.Application.Dtos.Users;
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
    : BaseController<IUserService, User, UserDto, int>(userService)
{
    private readonly IUserService _userService = userService;

    [HttpGet("GetAllOwners")]
    public async Task<IActionResult> GetAllOwners()
        => Ok(await _userService.GetAllUsersByRoleAsync((int)RoleNames.Owner));

    [HttpGet("GetAllSubscriptionAdmins")]
    public async Task<IActionResult> GetAllSubscriptionAdmins()
        => Ok(await _userService.GetAllUsersByRoleAsync((int)RoleNames.SubscriptionAdmin));

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto loginDto)
        => await HandleLoginAsync(loginDto, isCashier: false);

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var result = await _userService.ResetPasswordAsync(resetPasswordDto);

        return HandleResult(result, "Password reset successfully.", "Failed to reset password. User not found or invalid request.");
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
    {
        var result = await _userService.ForgotPasswordAsync(request);

        return HandleResult(result, "Password reset successfully.", "Failed to reset password. User not found or invalid request.");
    }

    private async Task<IActionResult> HandleLoginAsync(LoginDto loginDto, bool isCashier)
    {
        var loggedInDto = await _userService.LoginAsync(loginDto, isCashier);

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
