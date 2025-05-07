using AccessControlSystem.Application.Dtos.Cards;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Cards;

public class CardDtoValidator : AbstractValidator<CardDto>
{
    public CardDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
