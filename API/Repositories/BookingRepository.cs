using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
    {
        // Constructor
        public BookingRepository(BookingManagementDbContext context) : base(context) { }
    }
}
