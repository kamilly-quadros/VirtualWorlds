using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualWorlds.Server.Controllers;
using VirtualWorlds.Server.Data;
using VirtualWorlds.Server.Models;

namespace VirtualWorlds.Server.Tests.Controllers
{
    [TestClass]
    public class ShippingControllerTests
    {
        private VirtualWorldsDbContext? _context;
        private ShippingController? _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VirtualWorldsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new VirtualWorldsDbContext(options);

            SeedDatabase();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Shipping:Percentage", "0.1" } 
                })
                .Build();

            _controller = new ShippingController(_context, configuration);
        }

        private void SeedDatabase()
        {
            _context!.Books.AddRange(
                new Book
                {
                    Id = 1,
                    Name = "Livro A",
                    Price = 100
                },
                new Book
                {
                    Id = 2,
                    Name = "Livro B",
                    Price = 50
                }
            );

            _context.SaveChanges();
        }

        [TestMethod]
        public async Task CalculateShipping_DeveRetornarOk_ComCalculoCorreto()
        {
            var bookIds = new List<int> { 1, 2 };

            var result = await _controller.CalculateShipping(bookIds);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var value = okResult.Value;

            var totalBooksValue = (decimal)value.GetType()
                .GetProperty("totalBooksValue")!
                .GetValue(value)!;

            var shippingValue = (decimal)value.GetType()
                .GetProperty("shippingValue")!
                .GetValue(value)!;

            var totalWithShipping = (decimal)value.GetType()
                .GetProperty("totalWithShipping")!
                .GetValue(value)!;

            Assert.AreEqual(150m, totalBooksValue);
            Assert.AreEqual(15m, shippingValue);
            Assert.AreEqual(165m, totalWithShipping);
        }


        [TestMethod]
        public async Task CalculateShipping_IdsInvalidos_DeveRetornarBadRequest()
        {
            var bookIds = new List<int>();

            var result = await _controller.CalculateShipping(bookIds);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CalculateShipping_LivrosNaoEncontrados_DeveRetornarNotFound()
        {
            var bookIds = new List<int> { 999 };

            var result = await _controller.CalculateShipping(bookIds);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task CalculateShipping_QuandoErro_DeveRetornar500()
        {
            var options = new DbContextOptionsBuilder<VirtualWorldsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new VirtualWorldsDbContext(options);
            context.Dispose();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Shipping:Percentage", "0.1" }
                })
                .Build();

            var controller = new ShippingController(context, configuration);

            var result = await controller.CalculateShipping(new List<int> { 1 });

            Assert.IsInstanceOfType(result, typeof(ObjectResult));

            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }
    }
}
