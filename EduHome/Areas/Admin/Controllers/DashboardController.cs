﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles ="Admin, Moderator")]

public class DashboardController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
