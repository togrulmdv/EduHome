using AutoMapper;
using EduHome.DataTransferObjects;
using EduHome.Models.Identity;
using EduHome.Services.Interfaces;
using EduHome.ViewModels.AccountViewModels;
using EduHome.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers;

public class AccountController : Controller
{
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;
	private readonly IMailService _mailService;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public AccountController(IMapper mapper, UserManager<AppUser> userManager, IMailService mailService, IWebHostEnvironment webHostEnvironment)
	{
		_mapper = mapper;
		_userManager = userManager;
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

	public IActionResult Register()
	{
		if (User.Identity.IsAuthenticated)
			return BadRequest();

		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
	{
		if (User.Identity.IsAuthenticated)
			return BadRequest();

		if (!ModelState.IsValid)
			return View();

		AppUser newUser = _mapper.Map<AppUser>(registerViewModel);
		newUser.IsActive = true;

		IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);

		if (!identityResult.Succeeded)
		{
			foreach(var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View();
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

		var verificationLink = Url.Action("VerifyEmail", "Account", new { email = newUser.Email, token }, HttpContext.Request.Scheme);

		string body = await GetEmailTemplateAsync(verificationLink);

		MailRequest mailRequest = new MailRequest
		{
			ToEmail = registerViewModel.Email,
			Subject = "Verify your email",
			Body = body
		};

		await _mailService.SendEmailAsync(mailRequest);

		return RedirectToAction(nameof(SuccessRegistration));
	}

	public IActionResult SuccessRegistration()
	{
		return View();
	}

    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return View("ErrorRegistration");

        var result = await _userManager.ConfirmEmailAsync(user, token);

        return View(result.Succeeded ? nameof(VerifyEmail) : "ErrorRegistration");
    }

    public IActionResult ErrorRegistration()
    {
        return View();
    }

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleTwoFactorAuthentication()
	{
		var user = await _userManager.GetUserAsync(User);

		if (user is null)
			return NotFound();

		var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

		await _userManager.SetTwoFactorEnabledAsync(user, !isTwoFactorEnabled);

		return RedirectToAction("AccountDetail");
	}

	public async Task<IActionResult> AccountDetail()
	{
		if(!User.Identity.IsAuthenticated)
			return BadRequest();

		var user = await _userManager.GetUserAsync(User);

		if(user is null)
			return NotFound();

		return View(user);
	}
}