using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table(name: "tb_m_accounts")]  // Anotasi nama tabel accounts
    public class Account : BaseEntity
    {
        [Column(name: "password", TypeName = "nvarchar(max)")] // Anotasi nama kolom dan tipe data untuk models
        public string Password { get; set; }
        [Column(name: "otp")]
        public int Otp {  get; set; }
        [Column(name: "is_used")]
        public bool IsUsed { get; set; }
        [Column(name: "expired_date")]
        public DateTime ExpiredDate { get; set; }

        //Cardinality
        public ICollection<AccountRole>? AccountRoles { get; set; }
        public Employee? Employee { get; set; }
    }
}
