using EduHome.Models;

namespace EduHome.ViewModels.CourseViewModels;

public class CourseViewModel
{
    public IEnumerable<Course> Courses { get; set; }
    public IEnumerable<Event> Events { get; set; }
    public IEnumerable<Category> Categories { get; set; }
}
