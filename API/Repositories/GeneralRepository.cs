using API.Contracts;
using API.Data;

namespace API.Repositories
{
    // Implementasi interface IGeneralRepository
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public GeneralRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas baru dalam database
        public TEntity? Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();
                return entity; // Mengembalikan entitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas dari database
        public bool Delete(TEntity entity)
        {
            try
            {
                var universityDelete = _context.Set<TEntity>().Find(entity);
                if (universityDelete != null)
                {
                    _context.Set<TEntity>().Remove(universityDelete);
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

        // Mendapatkan semua entitas dalam database
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList(); // Mengembalikan daftar entitas dalam bentuk List
        }

        // Mendapatkan entitas berdasarkan GUID
        public TEntity? GetByGuid(Guid guid)
        {
            return _context.Set<TEntity>().Find(guid);
        }

        // Memperbarui entitas dalam database
        public bool Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
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
