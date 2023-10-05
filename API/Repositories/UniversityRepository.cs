using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
    {
        // Constructor
        public UniversityRepository(BookingManagementDbContext context) : base(context) { }

        // Melakukan pencarian universitas berdasarkan kode dan nama
        public University GetByCodeAndName(string universityCode, string universityName)
        {
            var university = GetContext().Set<University>()
                          .FirstOrDefault(u => u.Code == universityCode && u.Name == universityName);

            return university;
        }
    }
}
