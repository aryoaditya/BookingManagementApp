using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public EmployeeRepository(BookingManagementDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
