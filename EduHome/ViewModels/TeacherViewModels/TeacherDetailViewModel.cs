using EduHome.Models;

namespace EduHome.ViewModels.TeacherViewModels;

public class TeacherDetailViewModel
{
	public string Name { get; set; }
	public string ImageName { get; set; }
	public string Role { get; set; }
	public string Description { get; set; }
	public string Degree { get; set; }
	public string Experience { get; set; }
	public string Hobbies { get; set; }

	public string Faculty { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string Skype { get; set; }
	public List<SocialMedia> SocialMedias { get; set; }
	public List<TeacherSkillViewModel> TeacherSkills { get; set; }
}
