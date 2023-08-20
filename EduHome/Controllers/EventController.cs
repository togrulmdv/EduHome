using AutoMapper;
using EduHome.Areas.Admin.ViewModels.EventViewModels;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels.CourseViewModels;
using EduHome.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class EventController : Controller
{
	private readonly AppDbContext _context;
	private readonly IMapper _mapper;

	public EventController(AppDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IActionResult> Index()
	{
		var events = await _context.Events.OrderByDescending(obj => obj.CreatedDate).ToListAsync();
		List<EventCardViewModel> eventCardViewModels = _mapper.Map<List<EventCardViewModel>>(events);

		return View(eventCardViewModels);
	}

	public async Task<IActionResult> Detail(int id)
	{
		Event? events = await _context.Events.FirstOrDefaultAsync(crs => crs.Id == id);
		
		if (events is null)
		{
			return NotFound();
		}

		var eventDetailViewModel = _mapper.Map<EventDetailViewModel>(events);

		return View(eventDetailViewModel);
	}
}
