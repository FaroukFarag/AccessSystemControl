using AccessControlSystem.Application.Dtos.UnitAccessGroups;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.UnitAccessGroups;

public class UnitAccessGroupDtoValidator : AbstractValidator<UnitAccessGroupDto>
{
    public UnitAccessGroupDtoValidator()
    {
    }
}
