using API.DTOs.Accounts;
using API.DTOs.Employees;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Utilities.Validators.Accounts
{
    public class GeneralAccountValidator<T> : AbstractValidator<T> where T : GeneralAccounDto
    {
        public GeneralAccountValidator()
        {
            // Guid validation
            RuleFor(a => a.Guid)
                .NotEmpty();

            // Password validation
            RuleFor(a => a.Password)
                .NotEmpty()
                .MinimumLength(6);

            // Otp validation
            RuleFor(a => a.Otp)
                .NotNull();

            // IsUsed validation
            RuleFor(a => a.IsUsed)
                .NotNull();

            // ExpiredDate validation
            RuleFor(a => a.ExpiredDate)
                .NotEmpty();
        }
        
    }
}
