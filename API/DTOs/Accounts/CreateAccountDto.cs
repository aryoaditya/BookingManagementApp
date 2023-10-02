using API.DTOs.Bookings;
using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Accounts
{
    public class CreateAccountDto
    {
        public string Password { get; set; }
        public int Otp { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiredDate { get; set; }

        // Operator konversi implisit dari CreateAccountDto ke Account
        public static implicit operator Account(CreateAccountDto createAccountDto)
        {
            return new Account
            {
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
