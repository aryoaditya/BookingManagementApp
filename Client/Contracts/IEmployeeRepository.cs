﻿using API.Models;

namespace Client.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee, Guid>
    {
    }
}