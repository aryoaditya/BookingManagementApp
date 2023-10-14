using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validators.Employees
{
    public class UpdateValidator : GeneralEmployeeValidator<UpdateEmployeeDto>
    {
        public UpdateValidator() : base()
        {
            // Nik validation
            RuleFor(e => e.Nik)
               .NotEmpty()
               .MaximumLength(6);
        }
    }
}