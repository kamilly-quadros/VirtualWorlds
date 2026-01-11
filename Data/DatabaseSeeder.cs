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
            var booksDto = JsonSerializer.Deserialize<List<BookJsonDto>>(json)!;

            foreach (var dto in booksDto)
            {
                var author = context.Authors
                    .FirstOrDefault(a => a.NmAuthor == dto.Specifications.Author);

                if (author == null)
                {
                    author = new Author { NmAuthor = dto.Specifications.Author };
                    context.Authors.Add(author);
                }

                var book = new Book
                {
                    NmBook = dto.Name,
                    NrPriceBook = dto.Price
                };
                context.Books.Add(book);

                var specification = new Specification
                {
                    Author = author,
                    Book = book,
                    NrPageCount = dto.Specifications.PageCount,
                    DtOriginallyPublished = dto.Specifications.OriginallyPublished
                };
                context.Specifications.Add(specification);

                foreach (var illustratorName in NormalizeToList(dto.Specifications.Illustrator))
                {
                    var illustrator = context.Illustrators
                        .FirstOrDefault(i => i.NmIllustrator == illustratorName);

                    if (illustrator == null)
                    {
                        illustrator = new Illustrator { NmIllustrator = illustratorName };
                        context.Illustrators.Add(illustrator);
                    }

                    context.SpecificationIllustrators.Add(
                        new SpecificationIllustrator
                        {
                            Specification = specification,
                            Illustrator = illustrator
                        });
                }

                foreach (var genreName in NormalizeToList(dto.Specifications.Genres))
                {
                    var genre = context.Genres
                        .FirstOrDefault(g => g.NmGenre == genreName);

                    if (genre == null)
                    {
                        genre = new Genre { NmGenre = genreName };
                        context.Genres.Add(genre);
                    }

                    context.SpecificationGenres.Add(
                        new SpecificationGenre
                        {
                            Specification = specification,
                            Genre = genre
                        });
                }
            }

            await context.SaveChangesAsync();
        }

        private static IEnumerable<string> NormalizeToList(object value)
        {
            if (value is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Array)
                    return element.EnumerateArray().Select(x => x.GetString()!);

                if (element.ValueKind == JsonValueKind.String)
                    return new[] { element.GetString()! };
            }

            return Enumerable.Empty<string>();
        }
    }
}
