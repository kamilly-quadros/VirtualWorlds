using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_specification_genre")]
    public class SpecificationGenre
    {
        [Key]
        [Column("cd_specification_genre")]
        public int CdSpecificationGenre { get; set; }

        [Column("cd_specification")]
        public int CdSpecification { get; set; }
        public Specification Specification { get; set; } = null!;

        [Column("cd_genre")]
        public int CdGenre { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}
