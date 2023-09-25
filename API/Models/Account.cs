using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table(name: "tb_m_accounts")]
    public class Account : BaseEntity
    {
        [Column(name: "password", TypeName = "nvarchar(max)")]
        public string Password { get; set; }
        [Column(name: "otp")]
        public int Otp {  get; set; }
        [Column(name: "is_used")]
        public bool IsUsed { get; set; }
        [Column(name: "expired_date")]
        public DateTime ExpiredDate { get; set; }
    }
}
