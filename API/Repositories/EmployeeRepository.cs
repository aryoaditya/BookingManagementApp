using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        // Constructor
        public EmployeeRepository(BookingManagementDbContext context) : base(context) { }

        public string? GetLastNik()
        {
            var lastNik = GetContext().Set<Employee>()
                .OrderByDescending(e => e.Nik)
                .FirstOrDefault()?.Nik;

            return lastNik;
        }
    }
}
