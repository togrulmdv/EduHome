using AutoMapper;
using EduHome.Areas.Admin.ViewModels.TeacherViewModels;
using EduHome.Contexts;
using EduHome.ViewModels.TeacherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class TeacherController : Controller
{
	private readonly AppDbContext _context;
	private readonly IMapper _mapper;

	public TeacherController(AppDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IActionResult> Index()
	{
		var teachers = await _context.Teachers.OrderByDescending(t => t.CreatedDate).Include(t => t.SocialMedias).ToListAsync();
		List<TeacherCardViewModel> teacherCardViewModels = _mapper.Map<List<TeacherCardViewModel>>(teachers);

		return View(teacherCardViewModels);
	}

	public async Task<IActionResult> Detail(int id)
	{
		var teacher = await _context.Teachers.Include(t => t.SocialMedias).Include(t => t.TeacherSkills).ThenInclude(t => t.Skill).FirstOrDefaultAsync(t => t.Id == id);

		if (teacher is null)
		{
			return NotFound();
		}

		var teacherDetailViewModel = _mapper.Map<TeacherDetailViewModel>(teacher);

		return View(teacherDetailViewModel);
	}
}
