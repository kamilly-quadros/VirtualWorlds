using System.Text.Json;
using VirtualWorlds.Server.DTOs;
using VirtualWorlds.Server.Models;

namespace VirtualWorlds.Server.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(
            VirtualWorldsDbContext context,
            IWebHostEnvironment env)
        {
            if (context.Books.Any())
                return;

            var filePath = Path.Combine(env.ContentRootPath, "books.json");
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo books.json não encontrado em {filePath}");

            var json = await File.ReadAllTextAsync(filePath);

            var booksDto = JsonSerializer.Deserialize<List<BookJsonDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            )!;

            foreach (var dto in booksDto)
            {
                if (dto.Specifications == null)
                    throw new Exception($"Livro '{dto.Name}' está sem especificações.");

                var book = new Book
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price
                };

                context.Books.Add(book);
                await context.SaveChangesAsync();

                var specification = new Specification
                {
                    BookId = book.Id,
                    Author = dto.Specifications.Author,
                    PageCount = dto.Specifications.PageCount,
                    OriginallyPublished = dto.Specifications.OriginallyPublished,
                    IllustratorJson = SerializeToJsonArray(dto.Specifications.Illustrator),
                    GenresJson = SerializeToJsonArray(dto.Specifications.Genres)
                };

                context.Specifications.Add(specification);
            }

            await context.SaveChangesAsync();
        }

        private static string SerializeToJsonArray(object? value)
        {
            if (value == null)
                return "[]";

            if (value is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Array)
                {
                    var list = element.EnumerateArray()
                        .Select(e => e.GetString())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToList();

                    return JsonSerializer.Serialize(list);
                }

                if (element.ValueKind == JsonValueKind.String)
                {
                    return JsonSerializer.Serialize(new[] { element.GetString() });
                }
            }

            return "[]";
        }
    }
}
