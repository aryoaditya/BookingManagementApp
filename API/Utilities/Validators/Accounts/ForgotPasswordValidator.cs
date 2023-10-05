using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validators.Accounts
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
