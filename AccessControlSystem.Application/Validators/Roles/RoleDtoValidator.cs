using AccessControlSystem.Application.Dtos.Roles;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Roles;

public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
