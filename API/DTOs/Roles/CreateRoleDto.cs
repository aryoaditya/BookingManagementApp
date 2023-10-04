using API.DTOs.Rooms;
using API.Models;

namespace API.DTOs.Roles
{
    public class CreateRoleDto : GeneralRoleDto
    {
        // Operator konversi implisit dari CreateRoleDto ke Role
        public static implicit operator Role(CreateRoleDto createRoleDto)
        {
            return new Role
            {
                Name = createRoleDto.Name,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
