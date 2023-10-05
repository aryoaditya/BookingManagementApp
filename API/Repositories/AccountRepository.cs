using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    // Inherit dari GeneralRepository dan implementasi interface IAccountRepository
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        // Constructor
        public AccountRepository(BookingManagementDbContext context) : base(context) { }

        // Generate OTP
        public int GenerateRandomOtp()
        {
            // Seed untuk Random Number Generator
            int seed = Guid.NewGuid().GetHashCode();

            // Inisialisasi objek Random dengan seed
            Random random = new Random(seed);

            // Menghasilkan angka acak 6 digit
            int otp = random.Next(100000, 999999);

            return otp;
        }

        public Account? GetByEmployeeGuid(Guid employeeGuid)
        {
            return GetContext().Accounts.FirstOrDefault(a => a.Guid == employeeGuid);
        }
    }
}
