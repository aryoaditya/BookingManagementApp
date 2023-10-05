using API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        string? GetLastNik();
        IDbContextTransaction BeginTransaction();
        Employee? GetByEmail(string email);
    }
}
