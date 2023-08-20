namespace EduHome.Areas.Admin.ViewModels.BlogViewModels;

public class CreateBlogViewModel
{
	public string Name { get; set; }
	public IFormFile Image { get; set; }

	public string Description { get; set; }
}
