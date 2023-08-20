﻿using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]

public class SpeakerController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}