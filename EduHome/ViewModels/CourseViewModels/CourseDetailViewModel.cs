namespace EduHome.ViewModels.CourseViewModels;

public class CourseDetailViewModel
{
	public int Id { get; set; }
	public string ImageName { get; set; }
	public string Name { get; set; }
	public string ShortDescription { get; set; }
	public string LongDescription { get; set; }
	public DateTime StartDate { get; set; }
	public string Duration { get; set; }
	public string ClassDuration { get; set; }
	public string SkillLevel { get; set; }
	public string Language { get; set; }
	public int StudentCount { get; set; }
	public string Assesment { get; set; }
	public int Price { get; set; }
}
