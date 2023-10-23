using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handlers;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Contracts
{
    public interface IAuthRepository : IGeneralRepository<AccountDto, Guid>
    {
        Task<ResponseOkHandler<TokenDto>> Login(LoginDto loginDto);
    }
}