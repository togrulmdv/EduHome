using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CategoryViewModels;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils.Enums;
using EduHome.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CategoryController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _mapper = mapper;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        var categories = _context.Categories.IgnoreQueryFilters().OrderByDescending(c => c.CreatedDate).AsNoTracking().ToList();

        var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
        return View(categoryViewModel);
    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryViewModel)
    {

        ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (createCategoryViewModel is null)
        {
            return NotFound();
        }
        var newCategory = _mapper.Map<Category>(createCategoryViewModel);
        newCategory.IsDeleted = false;





        if (createCategoryViewModel.CourseId is not null)
        {
            List<CourseCategory> courses = new List<CourseCategory>();
            for (int i = 0; i < createCategoryViewModel.CourseId.Count(); i++)
            {
                CourseCategory courseCategory = new CourseCategory()
                {

                    CategoryId = newCategory.Id,
                    CourseId = createCategoryViewModel.CourseId[i]
                };

                courses.Add(courseCategory);
            }
            newCategory.CourseCategories = courses;
        }

        await _context.Categories.AddAsync(newCategory);

        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var Category = await _context.Categories.Include(c => c.CourseCategories).ThenInclude(c => c.Course).AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
        if (Category is null)
        {
            return BadRequest();
        }
        var detailCategoryViewModel = _mapper.Map<DetailCategoryViewModel>(Category);
        return View(detailCategoryViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        if (_context.Categories.Count() <= 3)
        {
            return BadRequest();
        }
        var Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
        if (Category is null)
        {
            return NotFound();
        }
        var categoryViewModel = _mapper.Map<CategoryViewModel>(Category);
        return View(categoryViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int Id)
    {
        if (_context.Categories.Count() <= 3)
        {
            return BadRequest();
        }
        var Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
        if (Category is null)
        {
            return NotFound();
        }
        Category.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");
        var Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Category is null)
        {
            return NotFound();
        }
        var updateCategoryViewModel = _mapper.Map<UpdateCategoryViewModel>(Category);


        return View(updateCategoryViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateCategoryViewModel updateCategoryViewModel, int Id)
    {
        ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");

        var Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
        List<CourseCategory> courseCategories = await _context.CourseCategories.ToListAsync();
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Category is null)
        {
            return NotFound();
        }



        if (updateCategoryViewModel.CourseId is not null)
        {

            courseCategories.RemoveAll(Category => Category.CategoryId == Category.Id);



            List<CourseCategory> newCourses = new List<CourseCategory>();

            for (int i = 0; i < updateCategoryViewModel.CourseId.Count(); i++)
            {
                CourseCategory courseCategory = new CourseCategory()
                {
                    CourseId = updateCategoryViewModel.CourseId[i],
                    CategoryId = Category.Id,
                };


                newCourses.Add(courseCategory);

            }
            Category.CourseCategories = newCourses;
        }


        Category = _mapper.Map(updateCategoryViewModel, Category);



        _context.Categories.Update(Category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}
