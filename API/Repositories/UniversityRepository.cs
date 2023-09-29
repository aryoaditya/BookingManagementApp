using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public UniversityRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Universitas baru dalam database
        public University? Create(University university)
        {
            try
            {
                _context.Set<University>().Add(university);
                _context.SaveChanges();
                return university; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Universitas dari database
        public bool Delete(University university)
        {
            try
            {
                _context.Set<University>().Remove(university);
                _context.SaveChanges();
                return true; // Mengembalikan true jika penghapusan berhasil
            }
            catch
            {
                return false; // Mengembalikan false jika terjadi kesalahan
            }
        }

        // Mendapatkan semua entitas Universitas dalam database
        public IEnumerable<University> GetAll()
        {
            return _context.Set<University>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Universitas berdasarkan GUID
        public University GetByGuid(Guid guid)
        {
            return _context.Set<University>().Find(guid);
        }

        // Memperbarui entitas Universitas dalam database
        public bool Update(University university)
        {
            try
            {
                _context.Set<University>().Update(university);
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
