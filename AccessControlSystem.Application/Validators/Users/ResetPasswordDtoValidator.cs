using AccessControlSystem.Application.Dtos.Users;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Users;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(rp => rp.UserName)
            .NotEmpty();

        RuleFor(rp => rp.OldPassword)
            .NotEmpty();

        RuleFor(rp => rp.NewPassword)
            .NotEmpty();
    }
}
