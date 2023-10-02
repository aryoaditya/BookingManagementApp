using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public BookingRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Booking baru dalam database
        public Booking? Create(Booking booking)
        {
            try
            {
                _context.Set<Booking>().Add(booking);
                _context.SaveChanges();
                return booking; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Booking dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var bookingDelete = _context.Set<Booking>().Find(guid);
                if (bookingDelete != null)
                {
                    _context.Set<Booking>().Remove(bookingDelete);
                    _context.SaveChanges();
                    return true; // Mengembalikan true jika penghapusan berhasil
                }
                return false; // Mengembalikan false jika data dengan GUID yang diberikan tidak ditemukan
            }
            catch
            {
                return false; // Mengembalikan false jika terjadi kesalahan saat penghapusan
            }
        }

        // Mendapatkan semua entitas Booking dalam database
        public IEnumerable<Booking> GetAll()
        {
            return _context.Set<Booking>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Booking berdasarkan GUID
        public Booking GetByGuid(Guid guid)
        {
            return _context.Set<Booking>().Find(guid);
        }

        // Memperbarui entitas Booking dalam database
        public bool Update(Booking booking)
        {
            try
            {
                _context.Set<Booking>().Update(booking);
                _context.SaveChanges();
                return true; // Mengembalikan true jika pembaruan berhasil
            }
            catch
            {
                return false; // Mengembalikan false jika terjadi kesalahan
            }
        }
    }
}
