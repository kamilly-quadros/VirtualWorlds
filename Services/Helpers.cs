using System.Text.Json;

namespace VirtualWorlds.Server.Services
{
    public static partial class Helpers
    {
        public static object DeserializeSingleOrList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return "";

            var element = JsonSerializer.Deserialize<JsonElement>(json);

            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString()!,
                JsonValueKind.Array => element.EnumerateArray()
                                           .Select(x => x.GetString()!)
                                           .ToList(),
                _ => ""
            };
        }

        public static List<string> DeserializeToList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new();

            var element = JsonSerializer.Deserialize<JsonElement>(json);

            return element.ValueKind switch
            {
                JsonValueKind.String => new() { element.GetString()! },
                JsonValueKind.Array => element.EnumerateArray()
                                              .Select(x => x.GetString()!)
                                              .ToList(),
                _ => new()
            };
        }
    }
}
