using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Services;
using VirtualWorlds.Server.Data;
using VirtualWorlds.Server.DTOs;
using System.ComponentModel;

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
            [FromQuery, DefaultValue("asc")] string? orderByPrice)
        {
            try
            {
                var query = _context.Books
                    .AsNoTracking()
                    .Include(b => b.Specifications)
                    .AsQueryable();

                query = Filters.ApplyName(query, name);
                query = Filters.ApplyAuthor(query, author);
                query = Filters.ApplyPriceOrder(query, orderByPrice);

                var books = await query.ToListAsync();

                books = Filters.ApplyYearRange(books, yearFrom, yearTo);
                books = Filters.ApplyJsonFilter(
                    books, illustrator, b => b.Specifications.IllustratorJson);
                books = Filters.ApplyJsonFilter(
                    books, genre, b => b.Specifications.GenresJson);

                var result = books.Select(b => new BookJsonDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    Specifications = new SpecificationJsonDto
                    {
                        OriginallyPublished = b.Specifications.OriginallyPublished,
                        Author = b.Specifications.Author,
                        PageCount = b.Specifications.PageCount,
                        Illustrator = Helpers.DeserializeSingleOrList(b.Specifications.IllustratorJson),
                        Genres = Helpers.DeserializeToList(b.Specifications.GenresJson)
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", details = ex.Message });
            }
        }
    }
}
