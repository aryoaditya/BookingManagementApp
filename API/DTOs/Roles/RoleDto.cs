using API.DTOs.Rooms;
using API.Models;

namespace API.DTOs.Roles
{
    public class RoleDto : GeneralRoleDto
    {
        public Guid Guid { get; set; }

        // Operator konversi eksplisit dari Role ke RoleDto
        public static explicit operator RoleDto(Role role)
        {
            return new RoleDto
            {
                Guid = role.Guid,
                Name = role.Name
            };
        }

        // Operator konversi implisit dari RoleDto ke Role
        public static implicit operator Role(RoleDto rolesDto)
        {
            return new Role
            {
                Guid = rolesDto.Guid,
                Name = rolesDto.Name,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
