using EduHome.Models;

namespace EduHome.ViewModels.TeacherViewModels;

public class TeacherCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string ImageName { get; set; }
    public List<SocialMedia> SocialMedias { get; set; }

}
