using API.Models;

namespace API.DTOs.AccountRoles
{
    public class AccountRoleDto
    {
        public Guid Guid { get; set; }
        public Guid AccountGuid { get; set; }
        public Guid RoleGuid { get; set; }

        // Operator konversi eksplisit dari AccountRole ke AccountRoleDto
        public static explicit operator AccountRoleDto(AccountRole accountRole)
        {
            return new AccountRoleDto
            {
                Guid = accountRole.Guid,
                AccountGuid = accountRole.AccountGuid,
                RoleGuid = accountRole.RoleGuid
            };
        }

        // Operator konversi implisit dari AccountRoleDto ke AccountRole
        public static implicit operator AccountRole(AccountRoleDto accountRoleDto)
        {
            return new AccountRole
            {
                Guid = accountRoleDto.Guid,
                AccountGuid = accountRoleDto.AccountGuid,
                RoleGuid = accountRoleDto.RoleGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
