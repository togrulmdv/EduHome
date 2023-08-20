namespace EduHome.Areas.Admin.ViewModels.CourseViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ImageName { get; set; }
    }
}
