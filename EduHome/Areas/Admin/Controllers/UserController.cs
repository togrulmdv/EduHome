using AutoMapper;
using EduHome.Areas.Admin.ViewModels.UserViewModels;
using EduHome.Contexts;
using EduHome.DataTransferObjects;
using EduHome.Models.Identity;
using EduHome.Services.Interfaces;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class UserController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IMailService mailService, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _mailService = mailService;
        _mapper = mapper;
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Index()
    {

        List<AppUser> users = await _context.Users.AsNoTracking().ToListAsync();
        List<UserViewModel> userViewModel = _mapper.Map<List<UserViewModel>>(users);
        return View(userViewModel);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Detail(string Id)
    {
        var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
        if (User is null)
            return NotFound();

        List<IdentityUserRole<string>> UserRole = _context.UserRoles.AsNoTracking().Where(ur => ur.UserId == User.Id).ToList();
        if (UserRole is null)
            return NotFound();

        List<IdentityRole> role = new List<IdentityRole>();
        for (int i = 0; i < UserRole.Count(); i++)
        {
            role.AddRange(_context.Roles.Where(r => r.Id == UserRole[i].RoleId).ToList());
        }




        if (role is null)
            return NotFound();


        DetailUserViewModel detailUserViewModel = _mapper.Map<DetailUserViewModel>(User);
        detailUserViewModel.Role = role;

        return View(detailUserViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole(string Id)
    {
        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");


        var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
        if (User is null)
            return NotFound();

        ChangeUserViewModel changeUserViewModel = _mapper.Map<ChangeUserViewModel>(User);
        return View(changeUserViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole(ChangeUserViewModel changeUserViewModel, string Id)
    {
        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");

        var User = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
        if (User is null)
        {
            return NotFound();
        }
        if (changeUserViewModel is null)
            return BadRequest();

        var userRole = _context.UserRoles.Where(ur => ur.UserId == Id).ToList();

        if (userRole is null)
        {
            return NotFound();
        }


        _context.UserRoles.RemoveRange(userRole);

        List<IdentityUserRole<string>> identityUserRole = new List<IdentityUserRole<string>>();

        for (int i = 0; i < changeUserViewModel.RoleId.Count(); i++)
        {
            IdentityUserRole<string> identityUR = new IdentityUserRole<string>();
            identityUR.RoleId = changeUserViewModel.RoleId[i];
            identityUR.UserId = userRole[0].UserId;
            identityUserRole.Add(identityUR);
        }

        await _context.UserRoles.AddRangeAsync(identityUserRole);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeStatus(string Id)
    {

        var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        if (User is null)
            return NotFound();

        StatusUserViewModel statusUserViewModel = _mapper.Map<StatusUserViewModel>(User);


        return View(statusUserViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeStatus(StatusUserViewModel statusUserViewModel, string Id)
    {
        var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        if (User is null)
            return NotFound();

        if (!User.IsActive)
        {
            User.IsActive = true;
        }
        else
        {
            User.IsActive = false;
        }


        _context.Users.Update(User);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Update(string Id)
    {

        var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (User is null)
        {
            return NotFound();
        }
        var updateUserViewModel = _mapper.Map<UpdateUserViewModel>(User);


        return View(updateUserViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateUserViewModel updateUserViewModel, string Id)
    {

        var User = await _context.Users.FirstOrDefaultAsync(c => c.Id == Id);
        User.NormalizedUserName = updateUserViewModel.UserName.ToUpper();
        User.NormalizedEmail = updateUserViewModel.Email.ToUpper();

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (User is null)
        {
            return NotFound();
        }






        User = _mapper.Map(updateUserViewModel, User);


        _context.Users.Update(User);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

}