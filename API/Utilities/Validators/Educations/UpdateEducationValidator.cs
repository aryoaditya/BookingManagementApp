using API.DTOs.Educations;
using FluentValidation;

namespace API.Utilities.Validators.Educations
{
    public class UpdateEducationValidator : GeneralEducationValidator<EducationDto>
    {
        public UpdateEducationValidator() : base ()
        {
            // Guid validation
            RuleFor(e => e.Guid)
                .NotEmpty();

        }
    }
}
