using API.DTOs.Accounts;
using System.Security.Claims;

namespace API.Contracts
{
    public interface ITokenHandler
    {
        string Generate(IEnumerable<Claim> claims);

        ClaimsDTO ExtractClaimsFromJwt(string token);
    }
}