﻿using API.Models;

namespace API.Contracts
{
    public interface IUniversityRepository : IGeneralRepository<University>
    {
        University GetByCodeAndName(string universityCode, string universityName);
    }
}