using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public RoomRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Room baru dalam database
        public Room? Create(Room room)
        {
            try
            {
                _context.Set<Room>().Add(room);
                _context.SaveChanges();
                return room; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Room dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var roomDelete = _context.Set<Room>().Find(guid);
                if (roomDelete != null)
                {
                    _context.Set<Room>().Remove(roomDelete);
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

        // Mendapatkan semua entitas Room dalam database
        public IEnumerable<Room> GetAll()
        {
            return _context.Set<Room>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Room berdasarkan GUID
        public Room GetByGuid(Guid guid)
        {
            return _context.Set<Room>().Find(guid);
        }

        // Memperbarui entitas Room dalam database
        public bool Update(Room room)
        {
            try
            {
                _context.Set<Room>().Update(room);
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
