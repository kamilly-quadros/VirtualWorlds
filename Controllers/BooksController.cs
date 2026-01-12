using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Business;
using VirtualWorlds.Server.Services;
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

            query = BookBusiness.ApplyName(query, name);
            query = BookBusiness.ApplyAuthor(query, author);
            query = BookBusiness.ApplyPriceOrder(query, orderByPrice);

            var books = await query.ToListAsync();

            books = BookBusiness.ApplyYearRange(books, yearFrom, yearTo);
            books = BookBusiness.ApplyJsonFilter(
                books, illustrator, b => b.Specifications.IllustratorJson);
            books = BookBusiness.ApplyJsonFilter(
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
                    Illustrator = Helpers.DeserializeSingleOrList(b.Specifications.IllustratorJson),
                    Genres = Helpers.DeserializeToList(b.Specifications.GenresJson)
                }
            });

            return Ok(result);
        }
    }
}
