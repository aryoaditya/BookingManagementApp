using API.DTOs.Accounts;
using API.Utilities.Handlers;
using Client.Contracts;
using Client.Models;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class AuthRepository : GeneralRepository<AccountDto, Guid>, IAuthRepository
    {
        public AuthRepository(string request = "Account/login/") : base(request)
        {
        }

        public async Task<ResponseOkHandler<TokenDto>> Login(LoginDto loginDto)
        {
            ResponseOkHandler<TokenDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOkHandler<TokenDto>>(apiResponse);
            }
            return entityVM;
        }
    }
}