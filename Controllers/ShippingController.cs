using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Data;
using VirtualWorlds.Server.Business;

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
            if (!ShippingBusiness.ValidateBookIds(bookIds, out var error1))
                return BadRequest(error1);

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

            if (!ShippingBusiness.ValidateBooksFound(books.Cast<object>().ToList(), out var error2))
                return NotFound(error2);

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
