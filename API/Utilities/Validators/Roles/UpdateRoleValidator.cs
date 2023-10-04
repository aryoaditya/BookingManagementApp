using API.DTOs.Roles;
using FluentValidation;

namespace API.Utilities.Validators.Roles
{
    public class UpdateRoleValidator : GeneralRoleValidator<RoleDto>
    {
        public UpdateRoleValidator() : base() 
        {
            // Guid validation
            RuleFor(r => r.Guid)
                .NotEmpty();
        }
    }
}
