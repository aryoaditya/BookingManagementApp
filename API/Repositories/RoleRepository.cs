using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public RoleRepository(BookingManagementDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
