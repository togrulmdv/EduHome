namespace EduHome.Areas.Admin.ViewModels.SpeakerViewModels;

public class DetailSpeakerViewModel
{
    public string Name { get; set; }
    public string ImageName { get; set; }
    public string Role { get; set; }
    public string Company { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<string> EventName { get; set; }
}
