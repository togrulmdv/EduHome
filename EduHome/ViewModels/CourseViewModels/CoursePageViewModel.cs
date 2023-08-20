using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.CategoryViewModels;

namespace EduHome.ViewModels.CourseViewModels;

public class CoursePageViewModel
{
	public IEnumerable<CourseCardViewModel> Courses { get; set; }
	public IEnumerable<BlogPostViewModel> Blogs { get; set; }
	public IEnumerable<CategoryViewModel> Categories { get; set; }
}
