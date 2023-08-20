namespace EduHome.Areas.Admin.ViewModels.CourseViewModels
{
    public class CreateCourseViewModel
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
        public DateTime StartDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assestment { get; set; }
        public List<int>? CategoryId { get; set; }
    }
}
