using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Subscriptions;
using AccessControlSystem.Application.Interfaces.Users;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Common.Extensions;
using AccessControlSystem.Common.Tokens.Interfaces;
using AccessControlSystem.Domain.Enums.Roles;
using AccessControlSystem.Domain.Interfaces.Repositories.Users;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupUnits;
using AccessControlSystem.Domain.Models.Roles;
using AccessControlSystem.Domain.Models.Shared;
using AccessControlSystem.Domain.Models.Units;
using AccessControlSystem.Domain.Models.Users;
using AccessControlSystem.Domain.Specifications.Absraction;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AccessControlSystem.Application.Services.Users;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ISubscriptionService subscriptionService,
    ITokensService tokensService,
    IOrderingService<User> orderingService) : BaseService<
        UserDto, UserDto, UserDto, UserDto, User, int>(
        userRepository, unitOfWork, mapper), IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly ISubscriptionService _subscriptionService = subscriptionService;
    private readonly ITokensService _tokensService = tokensService;
    private readonly IOrderingService<User> _orderingService = orderingService;

    private static readonly BaseSpecification<User> userWithUnitSpec = new()
    {
        IncludeChains =
        [
            new IncludeChain<User>
            {
                InitialInclude = u => u.Unit!,
                ThenIncludes =
                [
                    u => (u as Unit)!.Cards,
                ]
            },
            new IncludeChain<User>
            {
                InitialInclude = u => u.Unit!,
                ThenIncludes =
                [
                    u => (u as Unit)!.AccessGroupUnits,
                    agu => (agu as AccessGroupUnit)!.AccessGroup
                ]
            }
        ]
    };
    private static readonly Dictionary<string, Action<BaseSpecification<User>>> OrderingRules = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = spec => spec.OrderBy = s => s.UserName!,
        ["recent"] = spec => spec.OrderByDescending = s => s.CreatedAt,
    };

    public override async Task<ResultDto<UserDto>> CreateAsync(UserDto userDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create User",
            action: async () =>
            {
                await ValidateUserCreationAsync(userDto);

                var user = await CreateUserAsync(userDto);

                await AssignRoleToUserAsync(user, userDto.RoleId);

                return userDto;
            });
    }

    public override async Task<ResultDto<IEnumerable<UserDto>>> GetAllAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Users",
            action: async () =>
            {
                var users = await _userRepository.GetAllAsync();

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public override async Task<ResultDto<IEnumerable<UserDto>>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Paginated Users",
            action: async () =>
            {
                var paginatedModel = _mapper.Map<PaginatedModel>(paginatedModelDto);
                var users = await _userRepository.GetAllPaginatedAsync(paginatedModel);

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public async Task<ResultDto<UserDto>> GetUserByRoleAsync(int userId, int roleId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get User by Role",
            action: async () =>
            {
                var userIds = await GetUserIdsByRoleAsync(roleId);

                if (!userIds.Contains(userId))
                {
                    throw new InvalidOperationException("User not found in specified role");
                }

                var userWithIncludes = await _userRepository.GetAsync(userId, userWithUnitSpec);

                return _mapper.Map<UserDto>(userWithIncludes);
            });
    }

    public async Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Users by Role",
            action: async () =>
            {
                var userIds = await GetUserIdsByRoleAsync(roleId);
                var users = await GetUsersWithSpecificationAsync(CreateUsersByRoleSpec(userIds));

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public async Task<ResultDto<IEnumerable<UserDto>>> GetAllSubscriptionUsersByRoleAsync(int subscriptionId, int roleId)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Subscription Users by Role",
            action: async () =>
            {
                var userIds = await GetUserIdsByRoleAsync(roleId);
                var spec = CreateSubscriptionUsersByRoleSpec(userIds, subscriptionId);
                var users = await GetUsersWithSpecificationAsync(spec);

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public async Task<ResultDto<IEnumerable<UserDto>>> GetUnassignedOwnersAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Unassigned Owners",
            action: async () =>
            {
                var ownerRoleId = (int)RoleNames.Owner;
                var userIds = await GetUserIdsByRoleAsync(ownerRoleId);
                var spec = CreateUnassignedOwnersSpec(userIds);
                var users = await GetUsersWithSpecificationAsync(spec);

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public async Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId, string orderBy = "Recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Users by Role with Ordering",
            action: async () =>
            {
                var userIds = await GetUserIdsByRoleAsync(roleId);
                var spec = CreateUsersByRoleSpec(userIds);

                ApplyOrdering(spec, orderBy);

                var users = await GetUsersWithSpecificationAsync(spec);

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public async Task<ResultDto<IEnumerable<UserDto>>> GetAllSubscriptionUsersByRoleAsync(int subscriptionId, int roleId, string orderBy = "Recent")
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Subscription Users by Role with Ordering",
            action: async () =>
            {
                var userIds = await GetUserIdsByRoleAsync(roleId);
                var spec = CreateSubscriptionUsersByRoleSpec(userIds, subscriptionId);

                ApplyOrdering(spec, orderBy);

                var users = await GetUsersWithSpecificationAsync(spec);

                return _mapper.Map<IEnumerable<UserDto>>(users);
            });
    }

    public override async Task<ResultDto<UserDto>> UpdateAsync(UserDto newUserDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update User",
            action: async () =>
            {
                var existingUser = await _userManager.FindByIdAsync(newUserDto.Id.ToString())
                    ?? throw new InvalidOperationException("User not found");

                _mapper.Map(newUserDto, existingUser);

                if (string.IsNullOrEmpty(existingUser.SecurityStamp))
                {
                    existingUser.SecurityStamp = Guid.NewGuid().ToString();
                }

                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"User update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return newUserDto;
            });
    }

    public async Task<ResultDto<LoggedInDto>> LoginAsync(LoginDto model)
    {
        return await ExecuteServiceCallAsync(
            operationName: "User Login",
            action: async () =>
            {
                var user = await AuthenticateUserAsync(model)
                    ?? throw new InvalidOperationException("Authentication failed");
                var userWithUnits = await _userRepository.GetAsync(user.Id, userWithUnitSpec);
                var roles = await _userManager.GetRolesAsync(userWithUnits);
                var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault()!)
                    ?? throw new InvalidOperationException("Role not found");

                return new LoggedInDto
                {
                    UserId = userWithUnits.Id,
                    RoleId = role.Id,
                    SubscriptionId = userWithUnits.SubscriptionId,
                    Units = _mapper.Map<UnitDto>(userWithUnits.Unit),
                    Token = await GetToken(userWithUnits)
                };
            });
    }

    public async Task<ResultDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Reset Password",
            action: async () =>
            {
                var user = await _userManager.FindByNameAsync(resetPasswordDto.UserName)
                    ?? throw new InvalidOperationException("User not found");
                var result = await _userManager.ChangePasswordAsync(
                    user,
                    resetPasswordDto.OldPassword,
                    resetPasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Password reset failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return true;
            });
    }

    public async Task<ResultDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Forgot Password",
            action: async () =>
            {
                var user = await _userManager.FindByNameAsync(forgotPasswordDto.UserName)
                    ?? throw new InvalidOperationException("User not found");
                var removeResult = await _userManager.RemovePasswordAsync(user);

                if (!removeResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Password removal failed: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                }

                var addResult = await _userManager.AddPasswordAsync(user, forgotPasswordDto.NewPassword);

                if (!addResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Password set failed: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
                }

                return true;
            });
    }

    private async Task ValidateUserCreationAsync(UserDto userDto)
    {
        if (userDto.RoleId == (int)RoleNames.SubscriptionAdmin)
        {
            await ValidateSubscriptionAdminLimitAsync(userDto);
        }
    }

    private async Task ValidateSubscriptionAdminLimitAsync(UserDto userDto)
    {
        if (!userDto.SubscriptionId.HasValue)
        {
            throw new InvalidOperationException("Subscription ID is required for SubscriptionAdmin role");
        }

        var subscription = await _subscriptionService.GetAsync(userDto.SubscriptionId.Value);

        if (!subscription.Succeeded)
        {
            throw new InvalidOperationException("Failed to retrieve subscription information");
        }

        var adminCount = await GetSubscriptionAdminCountAsync(userDto.RoleId);

        if (subscription.ResultData.AdminNumber <= adminCount)
        {
            throw new InvalidOperationException("Number of Admins are Exceeded");
        }
    }

    private async Task<long> GetSubscriptionAdminCountAsync(int roleId)
    {
        var adminCountSpec = new BaseSpecification<User>
        {
            Criteria = u => u.UserRoles.Any(ur => ur.RoleId == roleId)
        };

        return await _userRepository.GetCountAsync(adminCountSpec);
    }

    private async Task<User> CreateUserAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var createResult = await _userManager.CreateAsync(user, userDto.Password!);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed: {errors}");
        }

        return user;
    }

    private async Task AssignRoleToUserAsync(User user, int roleId)
    {
        var role = await GetRoleByIdAsync(roleId);
        var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

        if (!roleResult.Succeeded)
        {
            await CleanupFailedUserCreationAsync(user);

            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));

            throw new InvalidOperationException($"Role assignment failed: {errors}");
        }
    }

    private async Task<Role> GetRoleByIdAsync(int roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());

        return role ?? throw new InvalidOperationException($"Role with ID {roleId} not found");
    }

    private async Task CleanupFailedUserCreationAsync(User user)
    {
        await _userManager.DeleteAsync(user);
    }

    private async Task<List<int>> GetUserIdsByRoleAsync(int roleId)
    {
        var role = await GetRoleByIdAsync(roleId);
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
        return usersInRole.Select(u => u.Id).ToList();
    }

    private async Task<IEnumerable<User>> GetUsersWithSpecificationAsync(BaseSpecification<User> spec)
    {
        return await _userRepository.GetAllAsync(spec);
    }

    private static BaseSpecification<User> CreateUsersByRoleSpec(IEnumerable<int> userIds)
    {
        return new BaseSpecification<User>
        {
            Criteria = u => userIds.Contains(u.Id)
        };
    }

    private static BaseSpecification<User> CreateSubscriptionUsersByRoleSpec(IEnumerable<int> userIds, int subscriptionId)
    {
        return new BaseSpecification<User>
        {
            Criteria = u => userIds.Contains(u.Id) && u.SubscriptionId == subscriptionId
        };
    }

    private static BaseSpecification<User> CreateUnassignedOwnersSpec(IEnumerable<int> userIds)
    {
        return new BaseSpecification<User>
        {
            Criteria = u => userIds.Contains(u.Id) && !u.UnitId.HasValue
        };
    }

    private void ApplyOrdering(BaseSpecification<User> spec, string orderBy)
    {
        _orderingService.ApplyOrdering(spec, OrderingRules, orderBy);
    }

    private async Task<User?> AuthenticateUserAsync(LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        return result.Succeeded ? await _userManager.FindByNameAsync(model.UserName) : null;
    }

    private async Task<string> GetToken(User user)
    {
        var claims = new List<TokenClaim>
        {
            new("userId", user.Id.ToString()),
            new("userName", user.UserName ?? string.Empty),
            new("email", user.Email ?? string.Empty),
            new("subscriptionId", user.SubscriptionId.ToString() ?? "0")
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in userRoles)
        {
            claims.Add(new TokenClaim(ClaimTypes.Role, role));
        }

        return await _tokensService.GenerateToken(claims)
            ?? throw new InvalidOperationException("Token generation failed");
    }
}
