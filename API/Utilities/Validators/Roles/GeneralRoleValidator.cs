using API.DTOs.Roles;
using FluentValidation;

namespace API.Utilities.Validators.Roles
{
    public class GeneralRoleValidator<T> : AbstractValidator<T> where T : GeneralRoleDto
    {
        public GeneralRoleValidator()
        {
            // Name validation
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
