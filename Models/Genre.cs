using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_genre")]
    public class Genre
    {
        [Key]
        [Column("cd_genre")]
        public int CdGenre { get; set; }

        [Column("nm_genre")]
        [Required]
        public string NmGenre { get; set; } = null!;

        public ICollection<SpecificationGenre> SpecificationGenres { get; set; } = new List<SpecificationGenre>();
    }
}
