using EduHome.Models.Identity;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts;

public class AppDbContextInitializer
{
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<AppUser> _userManager;
	private readonly AppDbContext _context;

	public AppDbContextInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, AppDbContext context)
	{
		_roleManager = roleManager;
		_userManager = userManager;
		_context = context;
	}

	public async Task InitializeAsync()
	{
		if (_context.Database.IsSqlServer())
		{
			await _context.Database.MigrateAsync();
		}
	}

	public async Task UserSeedAsync()
	{
		foreach (var role in Enum.GetValues(typeof(Roles)))
		{
			await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
		}

		AppUser admin = new AppUser
		{
			Name = "Admin",
			Surname = "Admin",
			UserName = "GOD",
			Email = "toghrulzm@code.edu.az",
			IsActive = true,
			EmailConfirmed = true
		};

		await _userManager.CreateAsync(admin, "Salam123!");

		await _userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
	}
}
