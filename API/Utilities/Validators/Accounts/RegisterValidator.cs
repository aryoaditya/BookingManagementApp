using API.DTOs.Accounts;
using API.Utilities.Enums;
using FluentValidation;

namespace API.Utilities.Validators.Accounts
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Birth date is required");
            
            RuleFor(x => x.Gender).IsInEnum().WithMessage("Gender is invalid");
            
            RuleFor(x => x.HiringDate).NotEmpty().WithMessage("Hiring date is required");
            
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format");
            
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            
            RuleFor(x => x.Major).NotEmpty().WithMessage("Major is required");
            
            RuleFor(x => x.Degree).NotEmpty().WithMessage("Degree is required");
            
            RuleFor(x => x.Gpa).InclusiveBetween(0, 4).WithMessage("GPA must be between 0 and 4");
            
            RuleFor(x => x.UniversityCode).NotEmpty().WithMessage("University code is required");
            
            RuleFor(x => x.UniversityName).NotEmpty().WithMessage("University name is required");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches(@"\d").WithMessage("Password must contain at least one number");
            
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Password and confirm password do not match");
        }
    }
}
