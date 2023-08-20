using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SettingViewModels;
using EduHome.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class SettingController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public SettingController(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    [Authorize(Roles = "Admin,Moderator")]
    public IActionResult Index()
    {
        var settings = _context.Settings.IgnoreQueryFilters().ToList();
        return View(settings);
    }
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Update(int Id)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == Id);
        if (setting is null)
        {
            return NotFound();
        }
        var updateSettingViewModel = _mapper.Map<UpdateSettingViewModel>(setting);

        return View(updateSettingViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Update(UpdateSettingViewModel updateSettingViewModel, int Id)
    {
        if (updateSettingViewModel is null)
        {
            return View();
        }

        var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == Id);
        if (setting is null)
        {
            return NotFound();
        }
        var checkUpdate = await _context.Settings.Where(s => s.Key == updateSettingViewModel.Key).ToListAsync();

        if (checkUpdate.Count() >= 1 && updateSettingViewModel.Key != setting.Key)
        {
            ModelState.AddModelError("Key", "Key already exists");
            return View();
        }

        setting = _mapper.Map(updateSettingViewModel, setting);

        _context.Settings.Update(setting);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }



}