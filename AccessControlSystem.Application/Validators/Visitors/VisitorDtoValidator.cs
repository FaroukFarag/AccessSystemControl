using AccessControlSystem.Application.Dtos.Visitors;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Visitors;

public class VisitorDtoValidator : AbstractValidator<VisitorDto>
{
    public VisitorDtoValidator()
    {
    }
}
