using System.Text.Json.Serialization;

namespace VirtualWorlds.Server.DTOs
{
    public class BookJsonDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("specifications")]
        public SpecificationJsonDto Specifications { get; set; } = null!;
    }
}
