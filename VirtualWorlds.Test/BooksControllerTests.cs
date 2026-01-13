using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualWorlds.Server.Controllers;
using VirtualWorlds.Server.Data;
using VirtualWorlds.Server.Models;
using VirtualWorlds.Server.DTOs;

namespace VirtualWorlds.Server.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTests
    {
        private VirtualWorldsDbContext? _context;
        private BooksController? _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VirtualWorldsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new VirtualWorldsDbContext(options);

            SeedDatabase();

            _controller = new BooksController(_context);
        }

        private void SeedDatabase()
        {
            _context!.Books.AddRange(
                new Book
                {
                    Id = 1,
                    Name = "Livro A",
                    Price = 50,
                    Specifications = new Specification
                    {
                        Author = "Autor 1",
                        OriginallyPublished = "2020",
                        PageCount = 200,
                        IllustratorJson = "[\"Ilustrador 1\"]",
                        GenresJson = "[\"Fantasia\"]"
                    }
                },
                new Book
                {
                    Id = 2,
                    Name = "Livro B",
                    Price = 30,
                    Specifications = new Specification
                    {
                        Author = "Autor 2",
                        OriginallyPublished = "2018",
                        PageCount = 150,
                        IllustratorJson = "[\"Ilustrador 2\"]",
                        GenresJson = "[\"Aventura\"]"
                    }
                }
            );

            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetBooks_DeveRetornarOkComLista()
        {
            var result = await _controller.GetBooks(
                name: null,
                author: null,
                illustrator: null,
                genre: null,
                yearFrom: null,
                yearTo: null,
                orderByPrice: "asc");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var books = okResult.Value as IEnumerable<BookJsonDto>;
            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Count());
        }

        [TestMethod]
        public async Task GetBooks_OrderByPriceAsc_DeveOrdenarCorretamente()
        {
            var result = await _controller.GetBooks(
                null, null, null, null, null, null, "asc");

            var okResult = result as OkObjectResult;
            var books = okResult.Value as IEnumerable<BookJsonDto>;

            Assert.AreEqual(30, books.First().Price);
        }

        [TestMethod]
        public async Task GetBooks_OrderByPriceDesc_DeveOrdenarCorretamente()
        {
            var result = await _controller.GetBooks(
                null, null, null, null, null, null, "desc");

            var okResult = result as OkObjectResult;
            var books = okResult.Value as IEnumerable<BookJsonDto>;

            Assert.AreEqual(50, books.First().Price);
        }

        [TestMethod]
        public async Task GetBooks_QuandoErro_DeveRetornar500()
        {
            var options = new DbContextOptionsBuilder<VirtualWorldsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new VirtualWorldsDbContext(options);
            context.Dispose();

            var controller = new BooksController(context);

            var result = await controller.GetBooks(
                null, null, null, null, null, null, null);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));

            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

    }
}
