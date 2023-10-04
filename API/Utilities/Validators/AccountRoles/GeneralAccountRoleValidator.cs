using API.DTOs.AccountRoles;
using FluentValidation;
namespace API.Utilities.Validators.AccountRoles
{
    public class GeneralAccountRoleValidator<T> : AbstractValidator<T> where T : GeneralAccountRoleDto
    {
        public GeneralAccountRoleValidator()
        {
            // AccountGuid validation
            RuleFor(ar => ar.AccountGuid)
                .NotEmpty();

            // RoleGuid validation
            RuleFor(ar => ar.RoleGuid)
                .NotEmpty();
        }
    }
}
