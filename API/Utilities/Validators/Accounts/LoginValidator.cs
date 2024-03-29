﻿using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validators.Accounts
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please enter a password");
        }
    }
}
