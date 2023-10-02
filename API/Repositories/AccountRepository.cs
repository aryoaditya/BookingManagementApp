using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public AccountRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Account baru dalam database
        public Account? Create(Account account)
        {
            try
            {
                _context.Set<Account>().Add(account);
                _context.SaveChanges();
                return account; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Account dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var accountDelete = _context.Set<Account>().Find(guid);
                if (accountDelete != null)
                {
                    _context.Set<Account>().Remove(accountDelete);
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

        // Mendapatkan semua entitas Account dalam database
        public IEnumerable<Account> GetAll()
        {
            return _context.Set<Account>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Account berdasarkan GUID
        public Account GetByGuid(Guid guid)
        {
            return _context.Set<Account>().Find(guid);
        }

        // Memperbarui entitas Account dalam database
        public bool Update(Account account)
        {
            try
            {
                _context.Set<Account>().Update(account);
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
