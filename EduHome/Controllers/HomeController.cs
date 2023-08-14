using EduHome.Contexts;
using EduHome.Models;
using EduHome.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class HomeController : Controller
{
	private readonly AppDbContext _context;
	private readonly UserManager<AppUser> _userManager;

	public HomeController(AppDbContext context, UserManager<AppUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Subscribe(string email)
	{
		if (User.Identity.IsAuthenticated)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user is not null)
			{
				var email = user.Email;

				// Check if the email is already subscribed
				var existingSubscription = _context.Subscribes.FirstOrDefault(s => s.Email == email);

				if (existingSubscription == null)
				{
					// Create a new subscription
					var newSubscription = new Subscribe
					{
						Email = email,
						IsSubscribed = true
					};
					_context.Subscribes.Add(newSubscription);
					_context.SaveChanges();
				}
			}
		}

		// Check if the email is already subscribed
		var existingSubscription = await _context.Subscribes.FirstOrDefaultAsync(s => s.Email == email);

		if (existingSubscription != null)
		{
			// Update the existing subscription
			existingSubscription.IsSubscribed = true;
		}
		else
		{
			// Create a new subscription
			var newSubscription = new Subscribe
			{
				Email = email,
				IsSubscribed = true
			};
			await _context.Subscribes.AddAsync(newSubscription);
		}

		await _context.SaveChangesAsync();

		// Redirect or return a response
		return RedirectToAction("Index", "Home"); // Redirect to a suitable page
	}
}
