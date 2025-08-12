using AccessControlSystem.Application.Dtos.Shared;
using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Application.Interfaces.Users;

public interface IUserService : IBaseService<UserDto, UserDto, UserDto, UserDto,
    User, int>
{
    Task<ResultDto<UserDto>> GetUserByRoleAsync(int userId, int roleId);
    Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId);
    Task<ResultDto<IEnumerable<UserDto>>> GetAllUsersByRoleAsync(int roleId, string orderBy);
    Task<ResultDto<LoggedInDto>> LoginAsync(LoginDto model);
    Task<ResultDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<ResultDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
}
