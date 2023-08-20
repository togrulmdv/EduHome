using AutoMapper;
using EduHome.Contexts;
using EduHome.DataTransferObjects;
using EduHome.Models;
using EduHome.Models.Identity;
using EduHome.Services.Interfaces;
using EduHome.ViewModels.AccountViewModels;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.CourseViewModels;
using EduHome.ViewModels.EventViewModels;
using EduHome.ViewModels.HomeViewModels;
using EduHome.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduHome.Controllers;

public class HomeController : Controller
{
	private readonly AppDbContext _context;
	private readonly UserManager<AppUser> _userManager;
	private readonly IMapper _mapper;
	private readonly IMailService _mailService;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public HomeController(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper, IMailService mailService, IWebHostEnvironment webHostEnvironment)
	{
		_context = context;
		_userManager = userManager;
		_mapper = mapper;
		_mailService = mailService;
		_webHostEnvironment = webHostEnvironment;
	}

	private async Task<string> GetEmailTemplateAsync(string link)
	{
		string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "templates", "verifyemail.html");

		using StreamReader streamReader = new StreamReader(path);

		string result = await streamReader.ReadToEndAsync();

		return result.Replace("[link]", link);
	}

	public async Task<IActionResult> Index()
	{
		var sliders = await _context.Sliders.OrderByDescending(obj => obj.CreatedDate).Take(3).ToListAsync();
		List<SliderCardViewModel> sliderCardViewModel = _mapper.Map<List<SliderCardViewModel>>(sliders);

		var courses = await _context.Courses.OrderByDescending(obj => obj.CreatedDate).Take(3).ToListAsync();
		List<CourseCardViewModel> courseCardViewModel = _mapper.Map<List<CourseCardViewModel>>(courses);

		var events = await _context.Events.OrderByDescending(obj => obj.CreatedDate).Take(4).ToListAsync();
		List<EventCardViewModel> eventCardViewModel = _mapper.Map<List<EventCardViewModel>>(events);

		var blogs = await _context.Blogs.OrderByDescending(obj => obj.CreatedDate).Take(3).ToListAsync();
		List<BlogCardViewModel> blogCardViewModel = _mapper.Map<List<BlogCardViewModel>>(blogs);

		var homeViewModels = new HomeViewModel
		{
			Sliders = sliderCardViewModel,
			Courses = courseCardViewModel,
			Events = eventCardViewModel,
			Blogs = blogCardViewModel,
		};

		return View(homeViewModels);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Subscribe(string email)
	{
		if (User.Identity.IsAuthenticated)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user is null)
				return NotFound();

			var existingSubscription = await _context.Subscribes
				.FirstOrDefaultAsync(s => s.Email == user.Email);

			if (existingSubscription is null)
			{
				_context.Subscribes.Add(new Subscribe
				{
					Email = user.Email,
					IsSubscribed = true
				});
				await _context.SaveChangesAsync();
			}
		}
		else
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var existingSubscription = await _context.Subscribes
					.FirstOrDefaultAsync(s => s.Email == email);

			if (existingSubscription is null)
			{
				var token = Guid.NewGuid().ToString();

				var verificationLink = Url.Action("VerifyEmail", "Home", new { email = email, token }, HttpContext.Request.Scheme);

				string body = await GetEmailTemplateAsync(verificationLink);

				MailRequest mailRequest = new MailRequest
				{
					ToEmail = email,
					Subject = "Verify your email",
					Body = body
				};

				await _mailService.SendEmailAsync(mailRequest);

				RedirectToAction("Index", "Home");
			}
		}
		return RedirectToAction("Index", "Home");
	}

	public async Task<IActionResult> VerifyEmail(string token, string email)
	{
		if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
		{
			return BadRequest();
		}
		if (await _context.Subscribes.FirstOrDefaultAsync(s => s.Email == email) is not null)
		{
			return BadRequest();
		}

		Subscribe subscribeUser = new Subscribe()
		{
			Email = email,
			IsSubscribed = true
		};
		await _context.Subscribes.AddAsync(subscribeUser);
		await _context.SaveChangesAsync();

		return Ok("Success");
	}
}