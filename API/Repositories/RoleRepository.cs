using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        // Constructor
        public RoleRepository(BookingManagementDbContext context) : base(context) { }

        public Guid? GetDefaultRoleGuid()
        {
            return GetContext().Set<Role>().FirstOrDefault(r => r.Name == "user")?.Guid;
        }
    }
}