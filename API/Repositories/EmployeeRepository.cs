using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public EmployeeRepository(BookingManagementDbContext context)
        {
            _context = context;
        }

        // Membuat entitas Employee baru dalam database
        public Employee? Create(Employee employee)
        {
            try
            {
                _context.Set<Employee>().Add(employee);
                _context.SaveChanges();
                return employee; // Mengembalikan universitas yang baru saja dibuat
            }
            catch
            {
                return null; // Mengembalikan null jika terjadi kesalahan
            }
        }

        // Menghapus entitas Employee dari database
        public bool Delete(Guid guid)
        {
            try
            {
                var employeeDelete = _context.Set<Employee>().Find(guid);
                if (employeeDelete != null)
                {
                    _context.Set<Employee>().Remove(employeeDelete);
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

        // Mendapatkan semua entitas Employee dalam database
        public IEnumerable<Employee> GetAll()
        {
            return _context.Set<Employee>().ToList(); // Mengembalikan daftar universitas dalam bentuk List
        }

        // Mendapatkan entitas Employee berdasarkan GUID
        public Employee GetByGuid(Guid guid)
        {
            return _context.Set<Employee>().Find(guid);
        }

        // Memperbarui entitas Employee dalam database
        public bool Update(Employee employee)
        {
            try
            {
                _context.Set<Employee>().Update(employee);
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
