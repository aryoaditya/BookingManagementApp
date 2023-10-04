using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validators.Universities
{
    public class GeneralUniversityValidator<T> : AbstractValidator<T> where T : GeneralUniversityDto
    {
        public GeneralUniversityValidator()
        {
            // Code validation
            RuleFor(u => u.Code)
               .NotEmpty()
               .MaximumLength(50);

            // Name validation
            RuleFor(u => u.Name)
               .NotEmpty()
               .MaximumLength(100);
        }
    }
}
