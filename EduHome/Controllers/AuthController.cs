using EduHome.DataTransferObjects;
using EduHome.Models.Identity;
using EduHome.Services.Interfaces;
using EduHome.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers;

public class AuthController : Controller
{
	private readonly UserManager<AppUser> _userManager;
	private readonly SignInManager<AppUser> _signInManager;
	private readonly IMailService _mailService;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, IWebHostEnvironment webHostEnvironment)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_mailService = mailService;
		_webHostEnvironment = webHostEnvironment;
	}

	private async Task<string> GetEmailTemplateAsync(string link)
	{
		string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "templates", "resetpassword.html");

		using StreamReader streamReader = new StreamReader(path);

		string result = await streamReader.ReadToEndAsync();

		return result.Replace("[link]", link);
	}

	public IActionResult LogIn()
	{
		if (User.Identity.IsAuthenticated)
			return BadRequest();

		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> LogIn(LoginViewModel loginViewModel, string? returnUrl)
	{
		if (User.Identity.IsAuthenticated)
			return BadRequest();

		if (!ModelState.IsValid)
			return View();

		AppUser appUser = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail);

		if (appUser is null)
		{
			appUser = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);

			if (appUser is null)
			{
				ModelState.AddModelError("", "Username/Email or Password is incorrect");
				return View();
			}
		}

		if (!appUser.IsActive)
		{
			ModelState.AddModelError("", "Account is not active");
			return View();
		}

		var emailConfirmed = await _userManager.IsEmailConfirmedAsync(appUser);

		if (!emailConfirmed)
		{
			ModelState.AddModelError("", "Email is not verified");
		}

		var signInResult = await _signInManager.PasswordSignInAsync(appUser, loginViewModel.Password, loginViewModel.RememberMe, true);

		if (signInResult.IsLockedOut)
		{
			ModelState.AddModelError("", "You must try again after a minute");
			return View();
		}

		if (!signInResult.Succeeded)
		{
			ModelState.AddModelError("", "Username/Email or Password is incorrect");
			return View();
		}

		if (!appUser.LockoutEnabled)
		{
			appUser.LockoutEnabled = true;
			appUser.LockoutEnd = null;
			await _userManager.UpdateAsync(appUser);
		}

		if (returnUrl is not null)
			return Redirect(returnUrl);

		return RedirectToAction("Index", "Home");
	}

	public async Task<IActionResult> LogOut()
	{
		if (!User.Identity.IsAuthenticated)
			return BadRequest();

		await _signInManager.SignOutAsync();

		return RedirectToAction("Index", "Home");
	}

	public IActionResult ForgotPassword()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
	{
		if (!ModelState.IsValid)
			return View();

		var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

		if (user is null)
		{
			ModelState.AddModelError("Email", "User not found by this email");
			return View();
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);

		var link = Url.Action("ResetPassword", "Auth", new { email = forgotPasswordViewModel.Email, token = token }, HttpContext.Request.Scheme);

		string body = await GetEmailTemplateAsync(link);

		MailRequest mailRequest = new MailRequest
		{
			ToEmail = forgotPasswordViewModel.Email,
			Subject = "Reset your password",
			Body = body
		};

		await _mailService.SendEmailAsync(mailRequest);

		return RedirectToAction("LogIn");
	}

	public async Task<IActionResult> ResetPassword(string email, string token)
	{
		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
			return BadRequest();

		var user = await _userManager.FindByEmailAsync(email);

		if (user is null)
			return NotFound();

		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel, string email, string token)
	{
		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
			return BadRequest();

		if (!ModelState.IsValid)
			return View();

		var user = await _userManager.FindByEmailAsync(email);

		if (user is null)
			return NotFound();

		IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword);

		if (!identityResult.Succeeded)
		{
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View();
		}

		return RedirectToAction("LogIn");
	}
}