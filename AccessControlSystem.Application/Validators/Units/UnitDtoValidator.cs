using AccessControlSystem.Application.Dtos.Units;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Units;

public class UnitDtoValidator : AbstractValidator<UnitDto>
{
    public UnitDtoValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
