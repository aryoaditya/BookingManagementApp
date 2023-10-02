using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class AccountRoleRepository : IAccountRoleRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public AccountRoleRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas AccountRole baru dalam database
        public AccountRole? Create(AccountRole accountRole)
        {
            try
            {
                _context.Set<AccountRole>().Add(accountRole);
                _context.SaveChanges();
                return accountRole; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas AccountRole dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var accountRoleDelete = _context.Set<AccountRole>().Find(guid);
                if (accountRoleDelete != null)
                {
                    _context.Set<AccountRole>().Remove(accountRoleDelete);
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

        // Mendapatkan semua entitas AccountRole dalam database
        public IEnumerable<AccountRole> GetAll()
        {
            return _context.Set<AccountRole>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas AccountRole berdasarkan GUID
        public AccountRole GetByGuid(Guid guid)
        {
            return _context.Set<AccountRole>().Find(guid);
        }

        // Memperbarui entitas AccountRole dalam database
        public bool Update(AccountRole accountRole)
        {
            try
            {
                _context.Set<AccountRole>().Update(accountRole);
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
