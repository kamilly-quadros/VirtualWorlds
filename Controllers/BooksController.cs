using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Data;

namespace VirtualWorlds.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            [FromQuery] DateTime? publishedFrom,
            [FromQuery] DateTime? publishedTo)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(b => b.Specification)
                    .ThenInclude(s => s.Author)
                .Include(b => b.Specification)
                    .ThenInclude(s => s.SpecificationIllustrators)
                        .ThenInclude(si => si.Illustrator)
                .Include(b => b.Specification)
                    .ThenInclude(s => s.SpecificationGenres)
                        .ThenInclude(sg => sg.Genre)
                .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(b =>
                    EF.Functions.Like(b.NmBook, $"%{name}%"));
            }
            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b =>
                    b.Specification.Author.NmAuthor.Contains(author));
            }
            if (!string.IsNullOrWhiteSpace(illustrator))
            {
                query = query.Where(b =>
                    b.Specification.SpecificationIllustrators
                        .Any(si => si.Illustrator.NmIllustrator.Contains(illustrator)));
            }
            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(b =>
                    b.Specification.SpecificationGenres
                        .Any(sg => sg.Genre.NmGenre.Contains(genre)));
            }
            if (publishedFrom.HasValue)
            {
                query = query.Where(b =>
                    DateTime.Parse(b.Specification.DtOriginallyPublished) >= publishedFrom.Value);
            }
            if (publishedTo.HasValue)
            {
                query = query.Where(b =>
                    DateTime.Parse(b.Specification.DtOriginallyPublished) <= publishedTo.Value);
            }
            var result = await query
                .Select(b => new
                {
                    b.CdBook,
                    b.NmBook,
                    b.NrPriceBook,
                    PublishedAt = b.Specification.DtOriginallyPublished,
                    Author = b.Specification.Author.NmAuthor,
                    Illustrators = b.Specification.SpecificationIllustrators
                        .Select(i => i.Illustrator.NmIllustrator),
                    Genres = b.Specification.SpecificationGenres
                        .Select(g => g.Genre.NmGenre)
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
