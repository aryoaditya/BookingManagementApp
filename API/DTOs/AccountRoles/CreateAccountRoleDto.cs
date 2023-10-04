using API.DTOs.Accounts;
using API.Models;

namespace API.DTOs.AccountRoles
{
    public class CreateAccountRoleDto : GeneralAccountRoleDto
    {
        // Operator konversi implisit dari CreateAccountRoleDto ke AccountRole
        public static implicit operator AccountRole(CreateAccountRoleDto createAccountRoleDto)
        {
            return new AccountRole
            {
                AccountGuid = createAccountRoleDto.AccountGuid,
                RoleGuid = createAccountRoleDto.RoleGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
