using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Business;
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
            [FromQuery] int? yearTo,
            [FromQuery] string? orderByPrice)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(b => b.Specifications)
                .AsQueryable();

            query = BookQueryFilters.ApplyName(query, name);
            query = BookQueryFilters.ApplyAuthor(query, author);
            query = BookQueryFilters.ApplyPriceOrder(query, orderByPrice);

            var books = await query.ToListAsync();

            books = BookQueryFilters.ApplyYearRange(books, yearFrom, yearTo);
            books = BookQueryFilters.ApplyJsonFilter(
                books, illustrator, b => b.Specifications.IllustratorJson);
            books = BookQueryFilters.ApplyJsonFilter(
                books, genre, b => b.Specifications.GenresJson);
            
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
                    Illustrator = BookQueryFilters.DeserializeSingleOrList(b.Specifications.IllustratorJson),
                    Genres = BookQueryFilters.DeserializeToList(b.Specifications.GenresJson)
                }
            });

            return Ok(result);
        }
    }
}
