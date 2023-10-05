using API.Models;

namespace API.Contracts
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        int GenerateRandomOtp();
        Account? GetByEmployeeGuid(Guid employeeGuid);
    }
}
