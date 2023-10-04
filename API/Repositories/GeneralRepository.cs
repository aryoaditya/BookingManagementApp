using API.Contracts;
using API.Data;
using API.Utilities.Handlers;

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

        // Akses _context ke derived class
        protected BookingManagementDbContext GetContext()
        {
            return _context;
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
            catch(Exception ex)
            {
                throw new ExceptionHandler(ex.Message); // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas dari database
        public bool Delete(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
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
            var entity = _context.Set<TEntity>().Find(guid);
            _context.ChangeTracker.Clear();
            return entity;
        }

        // Memperbarui entitas dalam database
        public bool Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
