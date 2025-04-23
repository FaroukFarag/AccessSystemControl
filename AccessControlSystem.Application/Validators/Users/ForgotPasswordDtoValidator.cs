using AccessControlSystem.Application.Dtos.Users;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Users;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(fp => fp.UserName)
            .NotEmpty();

        RuleFor(fp => fp.NewPassword)
            .NotEmpty();
    }
}
