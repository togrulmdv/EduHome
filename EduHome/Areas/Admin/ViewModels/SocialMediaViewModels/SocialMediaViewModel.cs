using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Admin.ViewModels.SocialMediaViewModels;

public class SocialMediaViewModel
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public string URL { get; set; }
    public string IconName { get; set; }
}
