using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SkillViewModels;
using EduHome.Contexts;
using EduHome.Models;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class SkillController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;



    public SkillController(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;

        _context = context;
    }

    public IActionResult Index()
    {
        var Skills = _context.Skills.IgnoreQueryFilters().OrderByDescending(s => s.CreatedDate).AsNoTracking().ToList();

        var skillViewModel = _mapper.Map<List<SkillViewModel>>(Skills);

        return View(skillViewModel);
    }
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create(CreateSkillViewModel createSkillViewModel)
    {



        if (!ModelState.IsValid)
        {
            return View();
        }
        if (createSkillViewModel is null)
        {
            return NotFound();
        }
        var newSkill = _mapper.Map<Skill>(createSkillViewModel);
        newSkill.IsDeleted = false;
        await _context.Skills.AddAsync(newSkill);

        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var Skill = await _context.Skills.Include(c => c.TeacherSkills).ThenInclude(c => c.Teacher).AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
        if (Skill is null)
        {
            return BadRequest();
        }
        var detailSkillViewModel = _mapper.Map<DetailSkillViewModel>(Skill);
        return View(detailSkillViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        var Skill = await _context.Skills.FirstOrDefaultAsync(c => c.Id == Id);
        if (Skill is null)
        {
            return NotFound();
        }
        var deleteSkillViewModel = _mapper.Map<DeleteSkillViewModel>(Skill);
        return View(deleteSkillViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSkill(int Id)
    {
        var Skill = await _context.Skills.FirstOrDefaultAsync(c => c.Id == Id);
        if (Skill is null)
        {
            return NotFound();
        }
        Skill.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {

        var Skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Skill is null)
        {
            return NotFound();
        }
        var updateSkillViewModel = _mapper.Map<UpdateSkillViewModel>(Skill);


        return View(updateSkillViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateSkillViewModel updateSkillViewModel, int Id)
    {


        var Skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == Id);

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Skill is null)
        {
            return NotFound();
        }





        Skill = _mapper.Map(updateSkillViewModel, Skill);



        _context.Skills.Update(Skill);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}
