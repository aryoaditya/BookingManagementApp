using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
    {
        // Constructor
        public BookingRepository(BookingManagementDbContext context) : base(context) { }

        public IEnumerable<Booking> GetCurrentBookings()
        {
            // Mendapatkan daftar pemesanan yang berlangsung pada hari ini
            var currentBookings = GetContext().Bookings
                .Where(booking => booking.StartDate <= DateTime.Today && booking.EndDate >= DateTime.Today)
                .ToList();

            return currentBookings;
        }
    }
}