using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validators.AccountRoles
{
    public class UpdateAccountRole : GeneralAccountRoleValidator<AccountRoleDto>
    {
        public UpdateAccountRole() : base ()
        {
            // Guid validation
            RuleFor(ar => ar.Guid)
                .NotEmpty();
        }
    }
}
