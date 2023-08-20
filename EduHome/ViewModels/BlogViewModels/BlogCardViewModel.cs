using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.BlogViewModels;

public class BlogCardViewModel
{
	public int Id { get; set; }
	public string ImageName { get; set; }
	public DateTime Date { get; set; }
	public string Name { get; set; }
	public int? CommentCount { get; set; }
	public string CreatedBy { get; set; }
}