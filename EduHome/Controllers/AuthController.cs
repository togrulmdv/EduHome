using EduHome.Models.Identity;
using EduHome.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers;

public class AuthController : Controller
{
	private readonly UserManager<AppUser> _userManager;
	private readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

		if(appUser is null)
		{
			appUser = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);

			if (appUser is null)
			{
				ModelState.AddModelError("", "Username/Email or Password is incorrect");
				return View();
			}
		}

		var signInResult = await _signInManager.PasswordSignInAsync(appUser, loginViewModel.Password, loginViewModel.RememberMe, true);

		if(signInResult.IsLockedOut)
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
		if(!User.Identity.IsAuthenticated)
			return BadRequest();

		await _signInManager.SignOutAsync();

		return RedirectToAction("Index", "Home");
	}
}