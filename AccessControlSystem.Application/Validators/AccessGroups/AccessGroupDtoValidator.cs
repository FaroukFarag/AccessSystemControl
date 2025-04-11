using AccessControlSystem.Application.Dtos.AccessGroups;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.AccessGroups;

public class AccessGroupDtoValidator : AbstractValidator<AccessGroupDto>
{
    public AccessGroupDtoValidator()
    {
        RuleFor(ag => ag.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
