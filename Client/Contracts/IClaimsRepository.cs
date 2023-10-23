using API.DTOs.Accounts;
using API.Models;

namespace Client.Contracts
{
    public interface IClaimsRepository : IGeneralRepository<ClaimsDTO, Guid>
    {
    }
}