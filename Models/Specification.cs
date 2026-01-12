using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualWorlds.Server.Models
{
    [Table("tb_specification")]
    public class Specification
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        [Column("author")]
        public string Author { get; set; } = string.Empty;

        [Column("page_count")]
        public int PageCount { get; set; }

        [Column("originally_published")]
        public string OriginallyPublished { get; set; } = null!;
        [Column("illustrator_json")]
        public string IllustratorJson { get; set; } = "[]";

        [Column("genres_json")]
        public string GenresJson { get; set; } = "[]";
    }
}
