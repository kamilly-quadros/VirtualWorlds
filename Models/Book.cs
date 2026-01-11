using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_books")]
    public class Book
    {
        [Key]
        [Column("cd_book")]
        public int CdBook { get; set; }

        [Column("nm_book")]
        [Required]
        public string NmBook { get; set; } = null!;

        [Column("nr_price_book")]
        public decimal NrPriceBook { get; set; }

        public Specification Specification { get; set; } = null!;
    }
}
