using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EducationRepository : IEducationRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public EducationRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Education baru dalam database
        public Education? Create(Education education)
        {
            try
            {
                _context.Set<Education>().Add(education);
                _context.SaveChanges();
                return education; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Education dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var educationDelete = _context.Set<Education>().Find(guid);
                if (educationDelete != null)
                {
                    _context.Set<Education>().Remove(educationDelete);
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

        // Mendapatkan semua entitas Education dalam database
        public IEnumerable<Education> GetAll()
        {
            return _context.Set<Education>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Education berdasarkan GUID
        public Education GetByGuid(Guid guid)
        {
            return _context.Set<Education>().Find(guid);
        }

        // Memperbarui entitas Education dalam database
        public bool Update(Education education)
        {
            try
            {
                _context.Set<Education>().Update(education);
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
