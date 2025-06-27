using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Application.Interfaces.Users;

public interface IUserService : IBaseService<User, UserDto, int>
{
    Task<UserDto> GetUserByRoleAsync(int userId, int roleId);
    Task<IEnumerable<UserDto>> GetAllUsersByRoleAsync(int roleId);
    Task<LoggedInDto> LoginAsync(LoginDto model);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
}
