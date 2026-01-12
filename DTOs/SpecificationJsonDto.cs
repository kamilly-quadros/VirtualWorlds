using System.Text.Json.Serialization;

namespace VirtualWorlds.Server.DTOs
{
    public class SpecificationJsonDto
    {
        [JsonPropertyName("Author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("Page count")]
        public int PageCount { get; set; }

        [JsonPropertyName("Originally published")]
        public string OriginallyPublished { get; set; } = null!;

        [JsonPropertyName("Illustrator")]
        public object? Illustrator { get; set; }

        [JsonPropertyName("Genres")]
        public object? Genres { get; set; }
    }
}
