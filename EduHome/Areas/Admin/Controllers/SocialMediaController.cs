using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SocialMediaViewModels;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class SocialMediaController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;
    public SocialMediaController(IMapper mapper, IWebHostEnvironment webHostEnvironment, AppDbContext context, IFileService fileService)
    {
        _fileService = fileService;
        _context = context;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    [Authorize(Roles = "Admin,Moderator")]
    public IActionResult Index()
    {
        var SocialMedias = _context.SocialMedias.IgnoreQueryFilters().OrderByDescending(s => s.CreatedDate).AsNoTracking().ToList();

        var socialMediaViewModel = _mapper.Map<List<SocialMediaViewModel>>(SocialMedias);
        return View(socialMediaViewModel);
    }
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name");
        return View();

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Create(CreateSocialMediaViewModel createSocialMediaViewModel)
    {
        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View();
        }
        var socialMedia = _mapper.Map<SocialMedia>(createSocialMediaViewModel);




        await _context.SocialMedias.AddAsync(socialMedia);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));



    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var socialMedia = await _context.SocialMedias.Include(p => p.Teacher).FirstOrDefaultAsync(p => p.Id == Id);
        if (socialMedia is null)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return View();
        }


        DetailSocialMediaViewModel detailSocialMediaViewModel = _mapper.Map<DetailSocialMediaViewModel>(socialMedia);
        return View(detailSocialMediaViewModel);



    }
    public async Task<IActionResult> Update(int Id)
    {
        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "Id", "Name");

        var SocialMedia = await _context.SocialMedias.FirstOrDefaultAsync(p => p.Id == Id);
        if (SocialMedia is null)
            return NotFound();

        UpdateSocialMediaViewModel updateSocialMediaViewModel = _mapper.Map<UpdateSocialMediaViewModel>(SocialMedia);

        return View(updateSocialMediaViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateSocialMediaViewModel updateSocialMediaViewModel, int Id)
    {
        var socialMedia = await _context.SocialMedias.FirstOrDefaultAsync(p => p.Id == Id);

        if (socialMedia is null)
            return NotFound();

        socialMedia = _mapper.Map(updateSocialMediaViewModel, socialMedia);

        _context.SocialMedias.Update(socialMedia);




        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int Id)
    {
        var socialMedia = await _context.SocialMedias.FirstOrDefaultAsync(s => s.Id == Id);
        if (socialMedia is null)
        {
            return NotFound();
        }
        var deleteSocialMediViewModel = _mapper.Map<DeleteSocialMediaViewModel>(socialMedia);
        return View(deleteSocialMediViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSocialMedia(int Id)
    {
        var socialMedia = await _context.SocialMedias.FirstOrDefaultAsync(s => s.Id == Id);
        if (socialMedia is null)
        {
            return NotFound();
        }


        socialMedia.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
}