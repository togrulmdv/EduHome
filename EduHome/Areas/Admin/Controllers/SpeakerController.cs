using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SpeakerViewModels;
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
public class SpeakerController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;

    public SpeakerController(AppDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IFileService fileService)
    {
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _context = context;
    }
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Index()
    {
        var Speakers = await _context.Speakers.IgnoreQueryFilters().OrderByDescending(s => s.CreatedDate).AsNoTracking().ToListAsync();

        var speakerViewModel = _mapper.Map<List<SpeakerViewModel>>(Speakers);

        return View(speakerViewModel);
    }
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Create(CreateSpeakerViewModel createSpeakerViewModel)
    {

        ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");

        if (!ModelState.IsValid)
        {
            return View();
        }
        if (createSpeakerViewModel is null)
        {
            return NotFound();
        }
        string FileName = string.Empty;
        var newSpeaker = _mapper.Map<Speaker>(createSpeakerViewModel);
        newSpeaker.IsDeleted = false;

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "speaker");
        try
        {
            FileName = await _fileService.CreateFileAsync(createSpeakerViewModel.Image, path);

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

        newSpeaker.ImageName = FileName;
        if (createSpeakerViewModel.EventId is not null)
        {
            List<EventSpeaker> events = new List<EventSpeaker>();
            for (int i = 0; i < createSpeakerViewModel.EventId.Count(); i++)
            {
                EventSpeaker eventSpeaker = new EventSpeaker()
                {

                    EventId = createSpeakerViewModel.EventId[i],
                    SpeakerId = newSpeaker.Id,
                };

                events.Add(eventSpeaker);
            }
            newSpeaker.EventSpeakers = events;
        }

        await _context.Speakers.AddAsync(newSpeaker);
        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int Id)
    {
        var Speaker = await _context.Speakers.Include(e => e.EventSpeakers).ThenInclude(e => e.Event).AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);
        if (Speaker is null)
        {
            return BadRequest();
        }
        var detailSpeakerViewModel = _mapper.Map<DetailSpeakerViewModel>(Speaker);
        return View(detailSpeakerViewModel);

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        var Speaker = await _context.Speakers.FirstOrDefaultAsync(e => e.Id == Id);
        if (Speaker is null)
        {
            return NotFound();
        }
        var speakerViewModel = _mapper.Map<SpeakerViewModel>(Speaker);
        return View(speakerViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEvent(int Id)
    {
        var Speaker = await _context.Speakers.FirstOrDefaultAsync(e => e.Id == Id);
        if (Speaker is null)
        {
            return NotFound();
        }

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "speaker", Speaker.ImageName);
        _fileService.DeteleFile(path);
        Speaker.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));


    }
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(int Id)
    {
        ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");
        var Speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == Id);
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Speaker is null)
        {
            return NotFound();
        }
        var updateSpeakerViewModel = _mapper.Map<UpdateSpeakerViewModel>(Speaker);


        return View(updateSpeakerViewModel);


    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Admin")]
    public async Task<IActionResult> Update(UpdateSpeakerViewModel updateSpeakerViewModel, int Id)
    {
        ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name");

        var Speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == Id);
        List<EventSpeaker> EventSpeakers = await _context.EventSpeakers.ToListAsync();
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (Speaker is null)
        {
            return NotFound();
        }
        string FileName = Speaker.ImageName;

        if (updateSpeakerViewModel.Image is not null)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "speaker");
                _fileService.DeteleFile(Path.Combine(path, FileName));

                FileName = await _fileService.CreateFileAsync(updateSpeakerViewModel.Image, path);

                Speaker.ImageName = FileName;
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
        if (updateSpeakerViewModel.EventId is not null)
        {

            EventSpeakers.RemoveAll(Speaker => Speaker.SpeakerId == Speaker.Id);



            List<EventSpeaker> newSpeakers = new List<EventSpeaker>();

            for (int i = 0; i < updateSpeakerViewModel.EventId.Count(); i++)
            {
                EventSpeaker eventSpeaker = new EventSpeaker()
                {
                    EventId = updateSpeakerViewModel.EventId[i],
                    SpeakerId = Speaker.Id
                };


                newSpeakers.Add(eventSpeaker);

            }
            Speaker.EventSpeakers = newSpeakers;
        }


        Speaker = _mapper.Map(updateSpeakerViewModel, Speaker);
        Speaker.ImageName = FileName;


        _context.Speakers.Update(Speaker);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
}