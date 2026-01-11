using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_illustrator")]
    public class Illustrator
    {
        [Key]
        [Column("cd_illustrator")]
        public int CdIllustrator { get; set; }

        [Column("nm_illustrator")]
        [Required]
        public string NmIllustrator { get; set; } = null!;

        public ICollection<SpecificationIllustrator> SpecificationIllustrators { get; set; } = new List<SpecificationIllustrator>();
    }
}
