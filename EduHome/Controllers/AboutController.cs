using AutoMapper;
using EduHome.Contexts;
using EduHome.ViewModels.TeacherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class AboutController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AboutController(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public IActionResult Index()
	{
        var teachers = _context.Teachers.OrderByDescending(t => t.CreatedDate).Take(4).Include(t => t.SocialMedias).ToList();

        var teacherViewModel = _mapper.Map<List<TeacherCardViewModel>>(teachers);

        return View(teacherViewModel);
	}
}
