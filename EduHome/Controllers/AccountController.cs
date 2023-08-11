using AutoMapper;
using EduHome.Models.Identity;
using EduHome.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers;

public class AccountController : Controller
{
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;

	public AccountController(IMapper mapper, UserManager<AppUser> userManager)
	{
		_mapper = mapper;
		_userManager = userManager;
	}

	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
	{
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

		return RedirectToAction("LogIn");
	}
}
