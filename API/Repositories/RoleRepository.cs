using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public RoleRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Role baru dalam database
        public Role? Create(Role role)
        {
            try
            {
                _context.Set<Role>().Add(role);
                _context.SaveChanges();
                return role; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Role dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var roleDelete = _context.Set<Role>().Find(guid);
                if (roleDelete != null)
                {
                    _context.Set<Role>().Remove(roleDelete);
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

        // Mendapatkan semua entitas Role dalam database
        public IEnumerable<Role> GetAll()
        {
            return _context.Set<Role>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Role berdasarkan GUID
        public Role GetByGuid(Guid guid)
        {
            return _context.Set<Role>().Find(guid);
        }

        // Memperbarui entitas Role dalam database
        public bool Update(Role role)
        {
            try
            {
                _context.Set<Role>().Update(role);
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
