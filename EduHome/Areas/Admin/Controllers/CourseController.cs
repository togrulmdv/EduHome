using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CourseViewModels;
using EduHome.Contexts;
using EduHome.Exceptions;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.Utils.Enums;
//using EduHome.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]

public class CourseController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;
    public CourseController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IFileService fileService)
    {
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _context = context;
    }


    public IActionResult Index()
    {
        var courses = _context.Courses.IgnoreQueryFilters().OrderByDescending(c => c.CreatedDate).AsNoTracking().ToList();

        var courseViewModel = _mapper.Map<List<CourseViewModel>>(courses);

        return View(courseViewModel);
    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create(CreateCourseViewModel createCourseViewModel)
    {

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (createCourseViewModel is null)
        {
            return NotFound();
        }
        string FileName = string.Empty;
        var newCourse = _mapper.Map<Course>(createCourseViewModel);
        newCourse.IsDeleted = false;

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course");
        try
        {
            FileName = await _fileService.CreateFileAsync(createCourseViewModel.Image, path);

        }
        catch (FileTypeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();

        }
        catch (FileSizeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();
        }

        newCourse.ImageName = FileName;
        if (createCourseViewModel.CategoryId is not null)
        {
            List<CourseCategory> categories = new List<CourseCategory>();
            for (int i = 0; i < createCourseViewModel.CategoryId.Count(); i++)
            {
                CourseCategory courseCategory = new CourseCategory()
                {

                    CategoryId = createCourseViewModel.CategoryId[i],
                    CourseId = newCourse.Id,
                };

                categories.Add(courseCategory);
            }
            newCourse.CourseCategories = categories;
        }

        await _context.Courses.AddAsync(newCourse);

        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var Course = await _context.Courses.Include(c => c.CourseCategories).ThenInclude(c => c.Category).AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
        if (Course is null)
        {
            return BadRequest();
        }
        var detailCourseViewModel = _mapper.Map<DetailCourseViewModel>(Course);
        return View(detailCourseViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        if (_context.Courses.Count() <= 3)
        {
            return BadRequest();
        }
        var Course = await _context.Courses.FirstOrDefaultAsync(e => e.Id == Id);
        if (Course is null)
        {
            return NotFound();
        }
        var courseViewModel = _mapper.Map<CourseViewModel>(Course);
        return View(courseViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEvent(int Id)
    {
        if (_context.Courses.Count() <= 3)
        {
            return BadRequest();
        }
        var Course = await _context.Courses.FirstOrDefaultAsync(e => e.Id == Id);
        if (Course is null)
        {
            return NotFound();
        }

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", Course.ImageName);
        _fileService.DeteleFile(path);
        Course.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
        var Course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Course is null)
        {
            return NotFound();
        }
        var updateCourseViewModel = _mapper.Map<UpdateCourseViewModel>(Course);


        return View(updateCourseViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateCourseViewModel updateCourseViewModel, int Id)
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

        var Course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == Id);
        List<CourseCategory> courseCategories = await _context.CourseCategories.ToListAsync();
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Course is null)
        {
            return NotFound();
        }
        string FileName = Course.ImageName;

        if (updateCourseViewModel.Image is not null)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course");
                _fileService.DeteleFile(Path.Combine(path, FileName));

                FileName = await _fileService.CreateFileAsync(updateCourseViewModel.Image, path);

                Course.ImageName = FileName;
            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }
        }
        if (updateCourseViewModel.CategoryId is not null)
        {

            courseCategories.RemoveAll(Course => Course.CourseId == Course.Id);



            List<CourseCategory> newCategories = new List<CourseCategory>();

            for (int i = 0; i < updateCourseViewModel.CategoryId.Count(); i++)
            {
                CourseCategory courseCategory = new CourseCategory()
                {
                    CourseId = Course.Id,
                    CategoryId = updateCourseViewModel.CategoryId[i]
                };


                newCategories.Add(courseCategory);

            }
            Course.CourseCategories = newCategories;
        }


        Course = _mapper.Map(updateCourseViewModel, Course);
        Course.ImageName = FileName;


        _context.Courses.Update(Course);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}
