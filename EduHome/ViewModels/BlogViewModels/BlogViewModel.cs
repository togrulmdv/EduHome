namespace EduHome.ViewModels.BlogViewModels;

public class BlogViewModel
{
	public int Id { get; set; }
	public bool IsDeleted { get; set; }
	public string Name { get; set; }
	public string ImageName { get; set; }
	public string CreatedBy { get; set; }
	public DateTime CreatedDate { get; set; }
}
