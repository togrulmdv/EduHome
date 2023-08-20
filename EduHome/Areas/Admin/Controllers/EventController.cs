using AutoMapper;
using EduHome.Contexts;
using EduHome.DataTransferObjects;
using EduHome.Exceptions;
using EduHome.Models.Identity;
using EduHome.Models;
using EduHome.Services.Interfaces;
using EduHome.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduHome.Areas.Admin.ViewModels.EventViewModels;

namespace EduHome.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class EventController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMailService _mailService;
    public EventController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IFileService fileService, UserManager<AppUser> userManager, IMailService mailService)
    {
        _mailService = mailService;
        _userManager = userManager;
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
        _webHostEnvironment = webHostEnvironment;

    }
    public IActionResult Index()
    {
        var Events = _context.Events.IgnoreQueryFilters().OrderByDescending(e => e.CreatedDate).AsNoTracking().ToList();

        var adminEventViewModel = _mapper.Map<List<EventViewModel>>(Events);
        return View(adminEventViewModel);
    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Speakers = new SelectList(await _context.Speakers.ToListAsync(), "Id", "Name");

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create(CreateEventViewModel createEventViewModel)
    {

        ViewBag.Speakers = new SelectList(await _context.Speakers.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (createEventViewModel is null)
        {
            return NotFound();
        }
        string FileName = string.Empty;
        var newEvent = _mapper.Map<Event>(createEventViewModel);
        newEvent.IsDeleted = false;

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event");
        try
        {
            FileName = await _fileService.CreateFileAsync(createEventViewModel.Image, path);

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

        newEvent.ImageName = FileName;

        List<EventSpeaker> speakers = new List<EventSpeaker>();
        for (int i = 0; i < createEventViewModel.SpeakerId.Count(); i++)
        {
            EventSpeaker eventSpeaker = new EventSpeaker()
            {

                SpeakerId = createEventViewModel.SpeakerId[i],
                EventId = newEvent.Id,
            };

            speakers.Add(eventSpeaker);
        }
        newEvent.EventSpeakers = speakers;


        await _context.Events.AddAsync(newEvent);

        foreach (var subscribe in _context.Subscribes)
        {

            MailRequest mailRequest = new MailRequest()
            {
                Subject = "New Event",
                ToEmail = subscribe.Email,
                Body = $"{newEvent.Name} has been posted"
            };
            await _mailService.SendEmailAsync(mailRequest);
        }



        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    private async Task<string> GetEmailTemplate(string link)
    {
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Templates", "verifyemail.html");
        using StreamReader streamReader = new StreamReader(path);
        string result = await streamReader.ReadToEndAsync();
        return result.Replace("[link]", link);


    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var Event = await _context.Events.Include(e => e.EventSpeakers).ThenInclude(e => e.Speaker).AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);
        if (Event is null)
        {
            return BadRequest();
        }
        var DetailEventViewModel = _mapper.Map<DetailEventViewModel>(Event);
        return View(DetailEventViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        if (_context.Events.Count() <= 3)
        {
            return BadRequest();
        }
        var Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == Id);
        if (Event is null)
        {
            return NotFound();
        }
        var adminEventViewModel = _mapper.Map<EventViewModel>(Event);
        return View(adminEventViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> DeleteEvent(int Id)
    {
        if (_context.Events.Count() <= 3)
        {
            return BadRequest();
        }
        var Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == Id);
        if (Event is null)
        {
            return NotFound();
        }

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event", Event.ImageName);
        _fileService.DeteleFile(path);
        Event.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        ViewBag.Speakers = new SelectList(await _context.Speakers.ToListAsync(), "Id", "Name");
        var Event = await _context.Events.FirstOrDefaultAsync(s => s.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Event is null)
        {
            return NotFound();
        }
        var updateEventViewModel = _mapper.Map<UpdateEventViewModel>(Event);


        return View(updateEventViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateEventViewModel updateEventViewModel, int Id)
    {
        ViewBag.Speakers = new SelectList(await _context.Speakers.ToListAsync(), "Id", "Name");

        var Event = await _context.Events.FirstOrDefaultAsync(s => s.Id == Id);
        List<EventSpeaker> EventSpeakers = await _context.EventSpeakers.ToListAsync();
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Event is null)
        {
            return NotFound();
        }
        string FileName = Event.ImageName;

        if (updateEventViewModel.Image is not null)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "event");
                _fileService.DeteleFile(Path.Combine(path, FileName));

                FileName = await _fileService.CreateFileAsync(updateEventViewModel.Image, path);

                Event.ImageName = FileName;
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
        if (updateEventViewModel.SpeakerId is not null)
        {

            EventSpeakers.RemoveAll(Event => Event.EventId == Event.Id);



            List<EventSpeaker> newSpeakers = new List<EventSpeaker>();

            for (int i = 0; i < updateEventViewModel.SpeakerId.Count(); i++)
            {
                EventSpeaker eventSpeaker = new EventSpeaker()
                {
                    EventId = Event.Id,
                    SpeakerId = updateEventViewModel.SpeakerId[i]
                };


                newSpeakers.Add(eventSpeaker);

            }
            Event.EventSpeakers = newSpeakers;
        }


        Event = _mapper.Map(updateEventViewModel, Event);
        Event.ImageName = FileName;


        _context.Events.Update(Event);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}