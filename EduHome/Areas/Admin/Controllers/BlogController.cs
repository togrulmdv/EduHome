using AutoMapper;
using EduHome.Areas.Admin.ViewModels.BlogViewModels;
using EduHome.Contexts;
using EduHome.Exceptions;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.Utils.Enums;
using EduHome.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class BlogController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;
    public BlogController(IMapper mapper, IWebHostEnvironment webHostEnvironment, AppDbContext context, IFileService fileService)
    {
        _fileService = fileService;
        _context = context;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var Blogs = await _context.Blogs.IgnoreQueryFilters().AsNoTracking().OrderByDescending(b => b.CreatedDate).ToListAsync();
        var blogViewModel = _mapper.Map<List<BlogViewModel>>(Blogs);
        return View(blogViewModel);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Detail(int Id)
    {
        var Blog = _context.Blogs.FirstOrDefault(x => x.Id == Id);
        if (Blog is null)
        {
            return BadRequest();
        }
        var detailSliderViewModel = _mapper.Map<DetailBlogViewModel>(Blog);

        return View(detailSliderViewModel);
    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create(CreateBlogViewModel adminCreateBlogViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (adminCreateBlogViewModel is null)
        {
            return NotFound();
        }
        string FileName = string.Empty;
        var newBlog = _mapper.Map<Blog>(adminCreateBlogViewModel);
        newBlog.IsDeleted = false;

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog");
        try
        {
            FileName = await _fileService.CreateFileAsync(adminCreateBlogViewModel.Image, path);

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

        newBlog.ImageName = FileName;


        await _context.Blogs.AddAsync(newBlog);
        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        if (_context.Blogs.Count() <= 3)
        {
            return BadRequest();
        }
        var blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == Id);
        if (blog is null)
        {
            return NotFound();
        }
        var deleteBlogViewModel = _mapper.Map<DeleteBlogViewModel>(blog);
        return View(deleteBlogViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBlog(int Id)
    {
        if (_context.Blogs.Count() <= 3)
        {
            return BadRequest();
        }
        var blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == Id);
        if (blog is null)
        {
            return NotFound();
        }

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog", blog.ImageName);
        _fileService.DeteleFile(path);
        blog.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        var Blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Blog is null)
        {
            return NotFound();
        }
        var updateSliderViewModel = _mapper.Map<UpdateBlogViewModel>(Blog);


        return View(updateSliderViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateBlogViewModel updateBlogViewModel, int Id)
    {
        var Blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == Id);

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Blog is null)
        {
            return NotFound();
        }
        string FileName = Blog.ImageName;

        if (updateBlogViewModel.Image is not null)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "blog");
                _fileService.DeteleFile(Path.Combine(path, FileName));

                FileName = await _fileService.CreateFileAsync(updateBlogViewModel.Image, path);

                Blog.ImageName = FileName;
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
        Blog = _mapper.Map(updateBlogViewModel, Blog);

        Blog.ImageName = FileName;

        _context.Blogs.Update(Blog);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}
