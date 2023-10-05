using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validators.Accounts
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Otp)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("New password and confirm password do not match");
        }
    }
}
