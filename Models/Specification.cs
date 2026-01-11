using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_specification")]
    public class Specification
    {
        [Key]
        [Column("cd_specification")]
        public int CdSpecification { get; set; }

        [Column("cd_book")]
        public int CdBook { get; set; }
        public Book Book { get; set; } = null!;

        [Column("cd_author")]
        public int CdAuthor { get; set; }
        public Author Author { get; set; } = null!;

        [Column("dt_originally_published")]
        public string DtOriginallyPublished { get; set; } = null!;

        [Column("nr_page_count")]
        public int NrPageCount { get; set; }

        public ICollection<SpecificationIllustrator> SpecificationIllustrators { get; set; } = new List<SpecificationIllustrator>();
        public ICollection<SpecificationGenre> SpecificationGenres { get; set; } = new List<SpecificationGenre>();
    }
}
