using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table(name: "tb_m_rooms")]  // Anotasi nama tabel rooms
    public class Room : BaseEntity
    {
        [Column(name: "name", TypeName = "nvarchar(100)")]  // Anotasi nama kolom dan tipe data untuk models
        public string Name { get; set; }
        [Column(name: "floor")]
        public int Floor { get; set; }
        [Column(name: "capacity")]
        public int Capacity { get; set; }
    }
}
