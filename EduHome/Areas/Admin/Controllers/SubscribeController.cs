using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SliderViewModels;
using EduHome.Areas.Admin.ViewModels.SubscribeViewModels;
using EduHome.Areas.Admin.ViewModels.UserViewModels;
using EduHome.Contexts;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]

public class SubscribeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

	public SubscribeController(AppDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	[Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Index()
    {
        var subscribes = await _context.Subscribes.IgnoreQueryFilters().OrderByDescending(s => s.Id).AsNoTracking().ToListAsync();
        return View(subscribes);
    }

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> ChangeStatus(int Id)
	{

		var subscribes = await _context.Subscribes.FirstOrDefaultAsync(u => u.Id == Id);
		if (subscribes is null)
			return NotFound();

		StatusSubscribeViewModel statusSubscribeViewModel = _mapper.Map<StatusSubscribeViewModel>(subscribes);


		return View(statusSubscribeViewModel);
	}

	[HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeStatus(StatusSubscribeViewModel statusSubscribeViewModel, int Id)
    {
        var Subscribes = await _context.Subscribes.FirstOrDefaultAsync(u => u.Id == Id);
        if (User is null)
            return NotFound();

        if (!Subscribes.IsSubscribed)
        {
            Subscribes.IsSubscribed = true;
        }
        else
        {
            Subscribes.IsSubscribed = false;
        }


        _context.Subscribes.Update(Subscribes);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
