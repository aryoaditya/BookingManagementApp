using API.DTOs.Educations;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Utilities.Validators.Educations
{
    public class GeneralEducationValidator<T> : AbstractValidator<T> where T : GeneralEducationDto
    {
        public GeneralEducationValidator()
        {
            // Major validation
            RuleFor(e => e.Major)
                .NotEmpty();

            // Degree validation
            RuleFor(e => e.Degree)
                .NotEmpty();

            // Gpa validation
            RuleFor(e => e.Gpa)
                .NotEmpty();

            // UniversityGuid validation
            RuleFor(e => e.UniversityGuid)
                .NotEmpty();
        }
    }
}
