using API.DTOs.Accounts;
using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Client.Repositories
{
    public class ClaimsRepository : GeneralRepository<ClaimsDTO, Guid>, IClaimsRepository
    {
        public ClaimsRepository(string request = "Account/GetClaims/") : base(request)
        {
        }
    }
}