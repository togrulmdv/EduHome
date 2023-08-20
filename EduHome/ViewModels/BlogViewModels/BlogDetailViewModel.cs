using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.BlogViewModels;

public class BlogDetailViewModel
{
	public string ImageName { get; set; }
	public DateTime Date { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string CreatedBy { get; set; }
	public int? CommentCount { get; set; }
}
