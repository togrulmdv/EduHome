using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.CourseViewModels;
using EduHome.ViewModels.EventViewModels;
using EduHome.ViewModels.SliderViewModels;

namespace EduHome.ViewModels.HomeViewModels;

public class HomeViewModel
{
	public IEnumerable<SliderCardViewModel> Sliders { get; set; }
	public IEnumerable<CourseCardViewModel> Courses { get; set; }
	public IEnumerable<EventCardViewModel> Events { get; set; }
	public IEnumerable<BlogCardViewModel> Blogs { get; set; }
}