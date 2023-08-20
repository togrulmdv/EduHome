using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.CourseViewModels;

public class CourseCardViewModel
{
	public int Id { get; set; }
	public string ImageName { get; set; }
	public string Name { get; set; }
	public string ShortDescription { get; set; }
}
