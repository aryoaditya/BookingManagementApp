using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
    {
        // Constructor
        public UniversityRepository(BookingManagementDbContext context) : base(context) { }
    }
}
