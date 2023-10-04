namespace API.DTOs.Accounts
{
    public class GeneralAccounDto
    {
        public Guid Guid { get; set; }
        public string Password { get; set; }
        public int Otp { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
