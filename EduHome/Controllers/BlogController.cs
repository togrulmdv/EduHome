using AutoMapper;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class BlogController : Controller
{
	private readonly IMapper _mapper;
	private readonly AppDbContext _context;

	public BlogController(IMapper mapper, AppDbContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		var blogs = await _context.Blogs.OrderByDescending(obj => obj.CreatedDate).ToListAsync();
		List<BlogCardViewModel> blogCardViewModels = _mapper.Map<List<BlogCardViewModel>>(blogs);

		return View(blogCardViewModels);
	}

	public async Task<IActionResult> Detail(int id)
	{
		Blog? blogs = await _context.Blogs.FirstOrDefaultAsync(crs => crs.Id == id);

		if (blogs is null)
		{
			return NotFound();
		}

		var blogDetailViewModel = _mapper.Map<BlogDetailViewModel>(blogs);

		return View(blogDetailViewModel);
	}
}

