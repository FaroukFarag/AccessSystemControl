﻿using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Users;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Common.Tokens.Interfaces;
using AccessControlSystem.Domain.Interfaces.Repositories.Roles;
using AccessControlSystem.Domain.Interfaces.Repositories.Users;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AccessControlSystem.Application.Services.Users;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IRoleRepository roleRepository,
    ITokensService tokensService) :
    BaseService<User, UserDto, int>(userRepository, unitOfWork, mapper), IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ITokensService _tokensService = tokensService;

    public async override Task<UserDto> CreateAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        var userResult = await _userManager.CreateAsync(user, userDto.Password!);

        if (!userResult.Succeeded)
            return default!;

        return userDto;
    }

    public async override Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var usersDtos = _mapper.Map<IReadOnlyList<UserDto>>(users);

        return usersDtos;
    }

    public async override Task<IEnumerable<UserDto>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto)
    {
        var paginatedModel = _mapper.Map<PaginatedModel>(paginatedModelDto);
        var users = await _userRepository.GetAllPaginatedAsync(paginatedModel: paginatedModel);
        var usersDtos = _mapper.Map<IReadOnlyList<UserDto>>(users);

        return usersDtos;
    }

    public async override Task<UserDto> Update(UserDto newUserDto)
    {
        var existingUser = await _userManager.FindByIdAsync(newUserDto.Id.ToString());

        if (existingUser == null)
        {
            return default!;
        }

        _mapper.Map(newUserDto, existingUser);

        if (string.IsNullOrEmpty(existingUser.SecurityStamp))
        {
            existingUser.SecurityStamp = Guid.NewGuid().ToString();
        }

        var result = await _userManager.UpdateAsync(existingUser);

        if (!result.Succeeded)
            return default!;

        return newUserDto;
    }

    public async Task<LoggedInDto> LoginAsync(LoginDto model, bool isCashier)
    {
        var user = await AuthenticateUserAsync(model);

        if (user == null)
            return default!;

        return new LoggedInDto
        {
            UserId = user.Id,
            Token = await GetToken(user)
        };
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByNameAsync(resetPasswordDto.UserName);

        if (user == null)
            return false;

        var userUpdated = await _userManager.UpdateAsync(user);

        if (!userUpdated.Succeeded)
            return false!;

        var result = await _userManager.ChangePasswordAsync(
            user,
            resetPasswordDto.OldPassword,
            resetPasswordDto.NewPassword);

        return result.Succeeded;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByNameAsync(forgotPasswordDto.UserName);

        if (user == null)
            return false;

        var removePasswordResult = await _userManager.RemovePasswordAsync(user);

        if (!removePasswordResult.Succeeded)
            return false;

        var addPasswordResult = await _userManager.AddPasswordAsync(user, forgotPasswordDto.NewPassword);

        return addPasswordResult.Succeeded;
    }

    private async Task<User> AuthenticateUserAsync(LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
            return null!;

        return (await _userManager.FindByNameAsync(model.UserName))!;
    }
    private async Task<string> GetToken(User user)
    {
        var claims = new List<TokenClaim>
        {
            new("userId", user.Id.ToString()),
            new("userName", user.UserName!),
            new("email", user.Email!)
        };

        return await _tokensService.GenerateToken(claims);
    }
}
