using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RoomRepository : GeneralRepository<Room>, IRoomRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public RoomRepository(BookingManagementDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
