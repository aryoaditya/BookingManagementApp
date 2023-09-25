using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table(name: "tb_m_educations")]  // Anotasi nama tabel educations
public class Education : BaseEntity
{
    [Column(name: "major", TypeName = "nvarchar(100)")] // Anotasi nama kolom dan tipe data untuk models
    public string Major { get; set; }
    [Column(name: "degree", TypeName = "nvarchar(100)")]
    public string Degree { get; set; }
    [Column(name: "gpa", TypeName = "real")]
    public float Gpa { get; set; }
    [Column(name: "university_guid")]
    public Guid UniversityGuid { get; set; }
}