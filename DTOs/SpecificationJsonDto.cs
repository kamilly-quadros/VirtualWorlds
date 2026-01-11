using System.Text.Json.Serialization;

namespace VirtualWorlds.Server.DTOs
{
    public class SpecificationJsonDto
    {
        public string Author { get; set; } = null!;
        public int PageCount { get; set; }

        [JsonPropertyName("Originally published")]
        public string OriginallyPublished { get; set; } = null!;

        public object Illustrator { get; set; } = null!;
        public object Genres { get; set; } = null!;
    }
}
