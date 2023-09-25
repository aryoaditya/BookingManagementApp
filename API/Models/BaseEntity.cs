using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    // Class berisi properti yang akan digunakan berulang
    public abstract class BaseEntity
    {
        [Key, Column(name: "guid")] // Anotasi sebagai primary key dan nama kolom
        public Guid Guid { get; set; }
        [Column(name: "created_date")]
        public DateTime CreatedDate { get; set; }
        [Column(name: "modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}