namespace EduHome.Areas.Admin.ViewModels.SpeakerViewModels;

public class UpdateSpeakerViewModel
{
    public IFormFile? Image { get; set; }
    public string Role { get; set; }
    public string Company { get; set; }
    public string Name { get; set; }
    public List<int>? EventId { get; set; }
}
