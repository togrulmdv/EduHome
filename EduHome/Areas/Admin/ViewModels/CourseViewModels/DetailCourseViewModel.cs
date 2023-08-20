using EduHome.Models;

namespace EduHome.Areas.Admin.ViewModels.CourseViewModels
{
    public class DetailCourseViewModel
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageName { get; set; }
        public DateTime StartDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assestment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<Category> Category { get; set; }
    }
}
