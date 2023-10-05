using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        // Constructor
        public EmployeeRepository(BookingManagementDbContext context) : base(context) { }

        // Mendapatkan Nik terakhir
        public string? GetLastNik()
        {
            var lastNik = GetContext().Set<Employee>()
                .OrderByDescending(e => e.Nik)
                .FirstOrDefault()?.Nik;

            return lastNik;
        }

        // Mulai transaksi
        public IDbContextTransaction BeginTransaction()
        {
            return GetContext().Database.BeginTransaction();
        }

        // Mendapatkan Email Employee
        public Employee? GetByEmail(string email)
        {
            // Menggunakan LINQ untuk mencari Employee berdasarkan email
            return GetContext().Set<Employee>()
                .FirstOrDefault(e => e.Email == email);
        }
    }
}
