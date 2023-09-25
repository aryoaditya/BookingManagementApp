using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table(name: "tb_m_account_roles")]  // Anotasi nama tabel account_roles
    public class AccountRole : BaseEntity
    {
        [Column(name: "account_guid")] // Anotasi nama kolom dan tipe data untuk models
        public Guid AccountGuid { get; set; }
        [Column(name: "role_guid")]
        public Guid RoleGuid { get; set; }
    }
}
