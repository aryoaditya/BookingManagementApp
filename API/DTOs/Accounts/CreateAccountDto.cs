using API.DTOs.Bookings;
using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Accounts
{
    public class CreateAccountDto : GeneralAccountDto
    {
        // Operator konversi implisit dari CreateAccountDto ke Account
        public static implicit operator Account(CreateAccountDto createAccountDto)
        {
            return new Account
            {
                Guid = createAccountDto.Guid,
                Password = createAccountDto.Password,
                Otp = createAccountDto.Otp,
                IsUsed = createAccountDto.IsUsed,
                ExpiredDate = createAccountDto.ExpiredDate,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
