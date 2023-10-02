using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public BookingRepository(BookingManagementDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
