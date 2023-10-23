using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Client.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "Employee/") : base(request)
        {
        }
    }
}