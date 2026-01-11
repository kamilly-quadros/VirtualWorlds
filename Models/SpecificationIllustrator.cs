using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_specification_illustrator")]
    public class SpecificationIllustrator
    {
        [Key]
        [Column("cd_specification_illustrator")]
        public int CdSpecificationIllustrator { get; set; }

        [Column("cd_specification")]
        public int CdSpecification { get; set; }
        public Specification Specification { get; set; } = null!;

        [Column("cd_illustrator")]
        public int CdIllustrator { get; set; }
        public Illustrator Illustrator { get; set; } = null!;
    }
}
