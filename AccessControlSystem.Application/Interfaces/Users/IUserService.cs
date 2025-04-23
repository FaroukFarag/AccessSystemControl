using AccessControlSystem.Application.Dtos.Users;
using AccessControlSystem.Application.Interfaces.Abstraction;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Application.Interfaces.Users;

public interface IUserService : IBaseService<User, UserDto, int>
{
    Task<LoggedInDto> LoginAsync(LoginDto model, bool isCashier);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
}
