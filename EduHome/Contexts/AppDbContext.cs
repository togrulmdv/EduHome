using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //public DbSet<Slider> Sliders { get; set; } = null!;
    //public DbSet<Shipping> Shippings { get; set; } = null!;
    //public DbSet<Product> Products { get; set; }
    //public DbSet<Category> Categories { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Shipping>().HasQueryFilter(shp => !shp.IsDeleted);
    //    base.OnModelCreating(modelBuilder);
    //}
}