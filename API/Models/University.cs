using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table(name:"tb_m_universities")]
public class University : BaseEntity
{
    [Column(name:"code", TypeName = "nvarchar(50)")]
    public string Code { get; set; }
    [Column(name: "name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }
}