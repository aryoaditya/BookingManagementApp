using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validators.Employees
{
    public class GeneralEmployeeValidator<T> : AbstractValidator<T> where T : GeneralEmployeeDto
    {
        public GeneralEmployeeValidator()
        {
            // FirstName validation
            RuleFor(e => e.FirstName)
               .NotEmpty();

            // BirthDate validation
            RuleFor(e => e.BirthDate)
               .NotEmpty()
               .LessThanOrEqualTo(DateTime.Now.AddYears(-18)).WithMessage("You are not old enough, the minimum age is 18"); // 18 years old

            // Gender validation
            RuleFor(e => e.Gender)
               .NotNull()
               .IsInEnum();

            // HiringDate validation
            RuleFor(e => e.HiringDate)
                .NotEmpty();

            // Email validation
            RuleFor(e => e.Email)
               .NotEmpty().WithMessage("Tidak Boleh Kosong")
               .EmailAddress().WithMessage("Format Email Salah");

            // PhoneNumber validation
            RuleFor(e => e.PhoneNumber)
               .NotEmpty()
               .MinimumLength(10)
               .MaximumLength(20);
        }
    }
}