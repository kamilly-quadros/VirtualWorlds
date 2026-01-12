using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Data;

namespace VirtualWorlds.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly VirtualWorldsDbContext _context;
        private readonly decimal _shippingPercentage;

        public ShippingController(
            VirtualWorldsDbContext context,
            IConfiguration configuration)
        {
            _context = context;

            _shippingPercentage =
                configuration.GetValue<decimal>("Shipping:Percentage");
        }

        [HttpGet]
        public async Task<IActionResult> CalculateShipping(
            [FromQuery] List<int> bookIds)
        {
            if (bookIds == null || bookIds.Count == 0)
                return BadRequest("Informe ao menos um código de livro.");

            var books = await _context.Books
                .AsNoTracking()
                .Where(b => bookIds.Contains(b.Id))
                .Select(b => new
                {
                    b.Id,
                    b.Name,
                    b.Price
                })
                .ToListAsync();

            if (!books.Any())
                return NotFound("Nenhum livro encontrado com os códigos informados.");

            var totalBooksValue = books.Sum(b => b.Price);
            var shippingValue = totalBooksValue * _shippingPercentage;

            var result = new
            {
                books,
                shippingPercentage = _shippingPercentage,
                totalBooksValue,
                shippingValue,
                totalWithShipping = totalBooksValue + shippingValue
            };

            return Ok(result);
        }
    }
}
