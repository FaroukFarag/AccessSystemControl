using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Application.Services.Shared;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Application.Interfaces.Users;

public interface IUserService : IBaseService<User, UserDto, int>
{
    Task<ResultDto<UserDto>> GetUserByRoleAsync(int userId, int roleId);
    Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId);
    Task<ResultDto<LoggedInDto>> LoginAsync(LoginDto model);
    Task<ResultDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<ResultDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
}
