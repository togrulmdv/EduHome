using EduHome.Models;
using EduHome.Models.Common;
using EduHome.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
	private readonly IHttpContextAccessor _contextAccessor;

	public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
	{
		_contextAccessor = contextAccessor;
	}

	public DbSet<Slider> Sliders { get; set; } = null!;
	public DbSet<Subscribe> Subscribes { get; set; } = null!;
	//public DbSet<Category> Categories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Slider>().HasQueryFilter(sldr => !sldr.IsDeleted);
		modelBuilder.Entity<Subscribe>().HasQueryFilter(sbscrb => !sbscrb.IsDeleted);
		base.OnModelCreating(modelBuilder);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		string? name = "Admin";

		var identity = _contextAccessor?.HttpContext?.User.Identity;

		if (identity is not null)
		{
			name = identity.IsAuthenticated ? identity.Name : "Admin";

		}

		var entries = ChangeTracker.Entries<BaseEntityAdditional>();

		foreach (var entry in entries)
		{

			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = name;
					entry.Entity.CreatedDate = DateTime.UtcNow;
					entry.Entity.UpdatedBy = name;
					entry.Entity.UpdateTime = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedBy = name;
					entry.Entity.UpdateTime = DateTime.UtcNow;
					break;
				default:
					break;

			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}