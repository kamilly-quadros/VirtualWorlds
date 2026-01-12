using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VirtualWorlds.Server.Models;

namespace VirtualWorlds.Server.Business
{
    public static partial class BookBusiness
    {
        public static IQueryable<Book> ApplyName(
            IQueryable<Book> query,
            string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return query;

            var nameLower = name.ToLower();

            return query.Where(b =>
                EF.Functions.Like(
                    b.Name.ToLower(),
                    $"%{nameLower}%"));
        }

        public static IQueryable<Book> ApplyAuthor(
            IQueryable<Book> query,
            string? author)
        {
            if (string.IsNullOrWhiteSpace(author))
                return query;

            var authorLower = author.ToLower();

            return query.Where(b =>
                b.Specifications.Author
                    .ToLower()
                    .Contains(authorLower));
        }

        public static IQueryable<Book> ApplyPriceOrder(
            IQueryable<Book> query,
            string? orderByPrice)
        {
            if (string.IsNullOrWhiteSpace(orderByPrice))
                return query;

            return orderByPrice.ToLower() switch
            {
                "asc" => query.OrderBy(b => b.Price),
                "desc" => query.OrderByDescending(b => b.Price),
                _ => query
            };
        }

        public static List<Book> ApplyYearRange(
            List<Book> books,
            int? yearFrom,
            int? yearTo)
        {
            if (!yearFrom.HasValue && !yearTo.HasValue)
                return books;

            return books
                .Where(b =>
                {
                    var year = TryGetYear(
                        b.Specifications.OriginallyPublished);

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

        public static List<Book> ApplyJsonFilter(
            List<Book> books,
            string? value,
            Func<Book, string> jsonSelector)
        {
            if (string.IsNullOrWhiteSpace(value))
                return books;

            return books
                .Where(b =>
                    JsonArrayContains(
                        jsonSelector(b),
                        value))
                .ToList();
        }

        private static bool JsonArrayContains(
            string json,
            string value)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            var element = JsonSerializer
                .Deserialize<JsonElement>(json);

            if (element.ValueKind == JsonValueKind.String)
            {
                return element.GetString()!
                    .Contains(value,
                        StringComparison.OrdinalIgnoreCase);
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                return element.EnumerateArray()
                    .Any(x =>
                        x.GetString()!
                            .Contains(value,
                                StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        private static int? TryGetYear(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return null;

            return DateTime.TryParse(date, out var parsed)
                ? parsed.Year
                : null;
        }
    }
}
