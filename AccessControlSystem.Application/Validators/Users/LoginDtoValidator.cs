using AccessControlSystem.Application.Dtos.Users;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Users;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.UserName)
            .NotEmpty();

        RuleFor(l => l.Password)
            .NotEmpty();
    }
}
