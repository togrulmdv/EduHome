namespace EduHome.Areas.Admin.ViewModels.SpeakerViewModels;

public class SpeakerViewModel
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public string ImageName { get; set; }
    public string Role { get; set; }
    public string Company { get; set; }
    public string Name { get; set; }
}
