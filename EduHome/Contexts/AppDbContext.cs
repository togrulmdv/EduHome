using EduHome.Models;
using EduHome.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Slider> Sliders { get; set; } = null!;
    //public DbSet<Shipping> Shippings { get; set; } = null!;
    //public DbSet<Product> Products { get; set; }
    //public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Slider>().HasQueryFilter(sldr => !sldr.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }
}