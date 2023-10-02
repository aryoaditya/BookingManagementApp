using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public UniversityRepository(BookingManagementDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
