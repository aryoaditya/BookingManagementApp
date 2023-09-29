using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table(name:"tb_m_universities")]  // Anotasi nama tabel universities
public class University : BaseEntity
{
    [Column(name:"code", TypeName = "nvarchar(50)")]  // Anotasi nama kolom dan tipe data untuk models
    public string Code { get; set; }
    [Column(name: "name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    // Cardinality
    public ICollection<Education>? Educations { get; set; }
}