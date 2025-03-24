﻿using AccessControlSystem.Application.Dtos.Users;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Users;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(15)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,15}$");

        RuleFor(u => u.PhoneNumber)
            .MaximumLength(15);
    }
}
