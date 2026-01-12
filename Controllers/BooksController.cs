using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VirtualWorlds.Server.Data;

namespace VirtualWorlds.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly VirtualWorldsDbContext _context;

        public BooksController(VirtualWorldsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(
        [FromQuery] string? name,
        [FromQuery] string? author,
        [FromQuery] string? illustrator,
        [FromQuery] string? genre,
        [FromQuery] int? yearFrom,
        [FromQuery] int? yearTo)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(b => b.Specifications)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameLower = name.ToLower();
                query = query.Where(b =>
                    EF.Functions.Like(
                        b.Name.ToLower(),
                        $"%{nameLower}%"));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                var authorLower = author.ToLower();
                query = query.Where(b =>
                    b.Specifications.Author.ToLower()
                        .Contains(authorLower));
            }

            var books = await query.ToListAsync();

            if (yearFrom.HasValue || yearTo.HasValue)
            {
                books = books
                    .Where(b =>
                    {
                        var year = TryGetYear(b.Specifications.OriginallyPublished);
                        if (!year.HasValue)
                            return false;

                        if (yearFrom.HasValue && year < yearFrom.Value)
                            return false;

                        if (yearTo.HasValue && year > yearTo.Value)
                            return false;

                        return true;
                    })
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(illustrator))
            {
                books = books.Where(b =>
                    JsonArrayContains(
                        b.Specifications.IllustratorJson,
                        illustrator))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                books = books.Where(b =>
                    JsonArrayContains(
                        b.Specifications.GenresJson,
                        genre))
                    .ToList();
            }

            var result = books.Select(b => new
            {
                id = b.Id,
                name = b.Name,
                price = b.Price,
                specifications = new
                {
                    Originally_published = b.Specifications.OriginallyPublished,
                    Author = b.Specifications.Author,
                    Page_count = b.Specifications.PageCount,
                    Illustrator = DeserializeSingleOrList(
                        b.Specifications.IllustratorJson),
                    Genres = DeserializeToList(
                        b.Specifications.GenresJson)
                }
            });

            return Ok(result);
        }

        private static bool JsonArrayContains(string json, string value)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            var element = JsonSerializer.Deserialize<JsonElement>(json);

            if (element.ValueKind == JsonValueKind.String)
            {
                return element.GetString()!
                    .Contains(value, StringComparison.OrdinalIgnoreCase);
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                return element.EnumerateArray()
                    .Any(x =>
                        x.GetString()!
                            .Contains(value, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        private static object DeserializeSingleOrList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return "";

            var element = JsonSerializer.Deserialize<JsonElement>(json);

            if (element.ValueKind == JsonValueKind.String)
                return element.GetString()!;

            if (element.ValueKind == JsonValueKind.Array)
                return element.EnumerateArray()
                    .Select(x => x.GetString()!)
                    .ToList();

            return "";
        }

        private static List<string> DeserializeToList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new();

            var element = JsonSerializer.Deserialize<JsonElement>(json);

            if (element.ValueKind == JsonValueKind.String)
                return new() { element.GetString()! };

            if (element.ValueKind == JsonValueKind.Array)
                return element.EnumerateArray()
                    .Select(x => x.GetString()!)
                    .ToList();

            return new();
        }
        private static int? TryGetYear(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return null;

            if (DateTime.TryParse(date, out var parsed))
                return parsed.Year;

            return null;
        }

    }
}
