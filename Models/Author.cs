using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_author")]
    public class Author
    {
        [Key]
        [Column("cd_author")]
        public int CdAuthor { get; set; }

        [Column("nm_author")]
        [Required]
        public string NmAuthor { get; set; } = null!;

        public ICollection<Specification> Specifications { get; set; } = new List<Specification>();
    }
}
