using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.TeacherViewModels;

public class DetailTeacherViewModel
{
    public string Name { get; set; }
    public string Role { get; set; }
    public string ImageName { get; set; }
    public string Description { get; set; }
    public string Degree { get; set; }
    public string Experience { get; set; }
    public string Hobbies { get; set; }
    public string Faculty { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Skype { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<float> Percentage { get; set; }
    public List<string>? SkillName { get; set; }
    public List<SocialMedia> SocialMedia { get; set; }
}
