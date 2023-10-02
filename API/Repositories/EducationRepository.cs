using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EducationRepository : GeneralRepository<Education>, IEducationRepository
    {
        private readonly BookingManagementDbContext _context;

        // Constructor
        public EducationRepository(BookingManagementDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
