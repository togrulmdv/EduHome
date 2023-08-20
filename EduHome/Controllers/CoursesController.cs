using AutoMapper;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.CategoryViewModels;
using EduHome.ViewModels.CourseViewModels;
using EduHome.ViewModels.HomeViewModels;
using EduHome.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class CoursesController : Controller
{
	private readonly AppDbContext _context;
	private readonly IMapper _mapper;

	public CoursesController(AppDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IActionResult> Index()
	{
		var courses = await _context.Courses.OrderByDescending(obj => obj.CreatedDate).ToListAsync();
		List<CourseCardViewModel> courseCardViewModel = _mapper.Map<List<CourseCardViewModel>>(courses);

		var blogs = await _context.Blogs.OrderByDescending(obj => obj.CreatedDate).Take(3).ToListAsync();
		List<BlogPostViewModel> blogPostViewModel = _mapper.Map<List<BlogPostViewModel>>(blogs);

		var category = await _context.Categories.OrderByDescending(obj => obj.CreatedDate).ToListAsync();
		List<CategoryViewModel> categoryViewModel = _mapper.Map<List<CategoryViewModel>>(category);

		var coursePageViewModels = new CoursePageViewModel
		{
			Courses = courseCardViewModel,
			Blogs = blogPostViewModel,
			Categories = categoryViewModel,
		};

		return View(coursePageViewModels);
	}

	public async Task<IActionResult> Detail(int id)
	{
		Course? courses = await _context.Courses.FirstOrDefaultAsync(crs => crs.Id == id);
		if (courses is null)
		{
			return NotFound();
		}

		var courseDetailViewModel = _mapper.Map<CourseDetailViewModel>(courses);

		return View(courseDetailViewModel);
	}

	public async Task<IActionResult> Filter(int id)
	{
		var courseCategory = await _context.CourseCategories.Include(cc => cc.Course).ThenInclude(cc => cc.CourseCategories).Where(c => c.CategoryId == id).ToListAsync();
		
		if (courseCategory.Count() == 0)
		{
			return RedirectToAction(nameof(Error));
		}
		
		var courseFilterViewModels = _mapper.Map<List<CourseFilterViewModel>>(courseCategory);

		return View(courseFilterViewModels);
	}

	public async Task<IActionResult> Error()
	{
		return View();
	}
}
