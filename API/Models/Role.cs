using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table(name: "tb_m_roles")]  // Anotasi nama tabel roles
    public class Role : BaseEntity
    {
        [Column(name: "name", TypeName = "nvarchar(100)")]  // Anotasi nama kolom dan tipe data untuk models
        public string Name { get; set; }

        // Cardinality
        public ICollection<AccountRole>? AccountRoles { get; set; }
    }
}
