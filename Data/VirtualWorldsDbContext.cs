using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Models;

namespace VirtualWorlds.Server.Data
{
    public class VirtualWorldsDbContext : DbContext
    {
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Illustrator> Illustrators => Set<Illustrator>();
        public DbSet<Specification> Specifications => Set<Specification>();
        public DbSet<SpecificationGenre> SpecificationGenres => Set<SpecificationGenre>();
        public DbSet<SpecificationIllustrator> SpecificationIllustrators => Set<SpecificationIllustrator>();
        public VirtualWorldsDbContext(DbContextOptions<VirtualWorldsDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Specification)
                .WithOne(s => s.Book)
                .HasForeignKey<Specification>(s => s.CdBook);
        }
    }
}
