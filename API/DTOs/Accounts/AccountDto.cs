using API.Models;

namespace API.DTOs.Accounts
{
    public class AccountDto : GeneralAccounDto
    {
        // Operator konversi eksplisit dari Account ke AccountDto
        public static explicit operator AccountDto(Account account)
        {
            return new AccountDto
            {
                Guid = account.Guid,
                Password = account.Password,
                Otp = account.Otp,
                IsUsed = account.IsUsed,
                ExpiredDate = account.ExpiredDate
            };
        }

        // Operator konversi implisit dari AccountDto ke Account
        public static implicit operator Account(AccountDto accountDto)
        {
            return new Account
            {
                Guid = accountDto.Guid,
                Password = accountDto.Password,
                Otp = accountDto.Otp,
                IsUsed = accountDto.IsUsed,
                ExpiredDate = accountDto.ExpiredDate,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
