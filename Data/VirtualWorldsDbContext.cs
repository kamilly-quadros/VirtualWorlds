using Microsoft.EntityFrameworkCore;
using VirtualWorlds.Server.Models;

namespace VirtualWorlds.Server.Data
{
    public class VirtualWorldsDbContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Specification> Specifications => Set<Specification>();
        public VirtualWorldsDbContext(DbContextOptions<VirtualWorldsDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Specifications)
                .WithOne()
                .HasForeignKey<Specification>("BookId");
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasConversion<double>();
        }
    }
}
