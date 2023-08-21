using AutoMapper;
using EduHome.Areas.Admin.ViewModels.TeacherViewModels;
using EduHome.Contexts;
using EduHome.Exceptions;
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
public class TeacherController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;
    public TeacherController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IFileService fileService)
    {
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        var teachers = _context.Teachers.AsNoTracking().OrderByDescending(t => t.CreatedDate).IgnoreQueryFilters().ToList();

        var teaherViewModels = _mapper.Map<List<TeacherViewModel>>(teachers);
        return View(teaherViewModels);
    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

        return View();
    }
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTeacherViewModel createTeacherViewModel)
    {


        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");


        if (!ModelState.IsValid)
        {
            return View(nameof(Percent), createTeacherViewModel);
        }
        if (createTeacherViewModel is null)
        {
            return NotFound();
        }


        string FileName = string.Empty;
        var newTeacher = _mapper.Map<Teacher>(createTeacherViewModel);
        newTeacher.IsDeleted = false;

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher");
        try
        {
            FileName = await _fileService.CreateFileAsync(createTeacherViewModel.Image, path);

        }
        catch (FileTypeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View(nameof(Percent), createTeacherViewModel);

        }
        catch (FileSizeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View(nameof(Percent), createTeacherViewModel);

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View(nameof(Percent), createTeacherViewModel);
        }

        newTeacher.ImageName = FileName;

        List<TeacherSkill> Skills = new List<TeacherSkill>();
        for (int i = 0; i < createTeacherViewModel.SkillId.Count(); i++)
        {
            TeacherSkill teacherSkill = new TeacherSkill()
            {

                TeacherId = newTeacher.Id,
                SkillId = createTeacherViewModel.SkillId[i],
                Percentage = createTeacherViewModel.Percentage[i],
            };

            Skills.Add(teacherSkill);
        }
        newTeacher.TeacherSkills = Skills;



        await _context.Teachers.AddAsync(newTeacher);

        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Admin,Moderator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Percent(CreateTeacherViewModel createTeacherViewModel)
    {
        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View(nameof(Create), createTeacherViewModel);
        }
        if (createTeacherViewModel is null)
        {
            return NotFound();
        }

        return View(createTeacherViewModel);




    }




    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var teacher = await _context.Teachers.Include(c => c.TeacherSkills).ThenInclude(c => c.Skill).Include(t => t.SocialMedias).FirstOrDefaultAsync(c => c.Id == Id);
        if (teacher is null)
        {
            return BadRequest();
        }
        var teacherSkills = await _context.TeacherSkills.Where(ts => ts.TeacherId == Id).ToListAsync();



        var detailTeacherViewModel = _mapper.Map<DetailTeacherViewModel>(teacher);
        for (int i = 0; i < teacherSkills.Count(); i++)
        {
            detailTeacherViewModel.Percentage[i] = teacherSkills[i].Percentage;
        }
        return View(detailTeacherViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        var Teacher = await _context.Teachers.FirstOrDefaultAsync(e => e.Id == Id);
        if (Teacher is null)
        {
            return NotFound();
        }
        var teacherViewModel = _mapper.Map<DeleteTeacherViewModel>(Teacher);
        return View(teacherViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTeacher(int Id)
    {
        var Teacher = await _context.Teachers.FirstOrDefaultAsync(e => e.Id == Id);
        if (Teacher is null)
        {
            return NotFound();
        }

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "course", Teacher.ImageName);
        _fileService.DeteleFile(path);
        Teacher.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");
        var Teacher = await _context.Teachers.Include(t => t.TeacherSkills).ThenInclude(t => t.Skill).FirstOrDefaultAsync(c => c.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Teacher is null)
        {
            return NotFound();
        }
        var percent = await _context.TeacherSkills.Where(ts => ts.TeacherId == Id).ToListAsync();

        var updateTeacherViewModel = _mapper.Map<UpdateTeacherViewModel>(Teacher);
        for (int i = 0; i < percent.Count(); i++)
        {
            updateTeacherViewModel.Percentage[i] = percent[i].Percentage;
        }

        return View(updateTeacherViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateTeacherViewModel updateTeacherViewModel, int Id)
    {
        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

        var Teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Id == Id);
        List<TeacherSkill> teacherSkills = await _context.TeacherSkills.Where(ts => ts.TeacherId == Id).ToListAsync();
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Teacher is null)
        {
            return NotFound();
        }
        string FileName = Teacher.ImageName;

        if (updateTeacherViewModel.Image is not null)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "teacher");
                _fileService.DeteleFile(Path.Combine(path, FileName));

                FileName = await _fileService.CreateFileAsync(updateTeacherViewModel.Image, path);

                Teacher.ImageName = FileName;
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

        if (updateTeacherViewModel.SkillId is not null)
        {

            TempData["TeacherId"] = Id;
            TempData["Image"] = FileName;
            return RedirectToAction(nameof(UpdatePercent), updateTeacherViewModel);


        }


        for (int i = 0; i < teacherSkills.Count(); i++)
        {
            teacherSkills[i].Percentage = updateTeacherViewModel.Percentage[i];
        }
        Teacher.TeacherSkills = teacherSkills;
        Teacher = _mapper.Map(updateTeacherViewModel, Teacher);


        Teacher.ImageName = FileName;



        _context.Teachers.Update(Teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> UpdatePercent(UpdateTeacherViewModel updateTeacherViewModel)
    {
        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View(nameof(Update), updateTeacherViewModel);
        }
        if (updateTeacherViewModel is null)
        {
            return NotFound();
        }

        return View(updateTeacherViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTeacher(UpdateTeacherViewModel updateTeacherViewModel)
    {
        ViewBag.Skills = new SelectList(await _context.Skills.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View(nameof(UpdatePercent));
        }
        if (updateTeacherViewModel is null)
        {
            return NotFound();
        }
        var Teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == (int)TempData["TeacherId"]);
        if (Teacher is null)
        {
            return NotFound();
        }
        var teacherSkills = await _context.TeacherSkills.Where(ts => ts.TeacherId == (int)TempData["TeacherId"]).ToListAsync();
        teacherSkills.RemoveAll(teacher => teacher.TeacherId == Teacher.Id);

        List<TeacherSkill> newSkills = new List<TeacherSkill>();

        for (int i = 0; i < updateTeacherViewModel.SkillId.Count(); i++)
        {
            TeacherSkill skillTeacher = new TeacherSkill()
            {
                TeacherId = Teacher.Id,
                SkillId = updateTeacherViewModel.SkillId[i],
                Percentage = updateTeacherViewModel.Percentage[i]
            };


            newSkills.Add(skillTeacher);

        }
        Teacher.TeacherSkills = newSkills;
        Teacher = _mapper.Map(updateTeacherViewModel, Teacher);
        Teacher.ImageName = (string)TempData["Image"];


        _context.Teachers.Update(Teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}