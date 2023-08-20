namespace EduHome.Areas.Admin.ViewModels.SkillViewModels;

public class DetailSkillViewModel
{
    public string Name { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<string> TeacherName { get; set; }
}
