﻿using EduHome.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public FooterViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
	{
        var settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

        return View(settings);
    }
}
