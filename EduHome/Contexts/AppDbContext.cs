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
	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Course> Courses { get; set; } = null!;
	public DbSet<CourseCategory> CourseCategories { get; set; } = null!;
	public DbSet<Speaker> Speakers { get; set; } = null!;
	public DbSet<Event> Events { get; set; } = null!;
	public DbSet<EventSpeaker> EventSpeakers { get; set; } = null!;
	public DbSet<Blog> Blogs { get; set; } = null!;
	public DbSet<Teacher> Teachers { get; set; } = null!;
	public DbSet<Skill> Skills { get; set; } = null!;
	public DbSet<TeacherSkill> TeacherSkills { get; set; } = null!;
	public DbSet<SocialMedia> SocialMedias { get; set; } = null!;
	public DbSet<Setting> Settings { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Slider>().HasQueryFilter(sldr => !sldr.IsDeleted);
		//modelBuilder.Entity<Subscribe>().HasQueryFilter(sbscrb => !sbscrb.IsDeleted);
		modelBuilder.Entity<Course>().HasQueryFilter(crs => !crs.IsDeleted);
		modelBuilder.Entity<Category>().HasQueryFilter(ctgry => !ctgry.IsDeleted);
		modelBuilder.Entity<Event>().HasQueryFilter(evnt => !evnt.IsDeleted);
		modelBuilder.Entity<Speaker>().HasQueryFilter(spkr => !spkr.IsDeleted);
		modelBuilder.Entity<Blog>().HasQueryFilter(blg => !blg.IsDeleted);
		modelBuilder.Entity<Teacher>().HasQueryFilter(tchr => !tchr.IsDeleted);
		modelBuilder.Entity<Skill>().HasQueryFilter(skl => !skl.IsDeleted);
		modelBuilder.Entity<SocialMedia>().HasQueryFilter(scl => !scl.IsDeleted);
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