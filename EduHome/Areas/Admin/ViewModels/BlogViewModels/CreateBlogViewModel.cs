using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Admin.ViewModels.BlogViewModels;

public class CreateBlogViewModel
{
    public IFormFile Image { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required, MaxLength(800)]
    public string Description { get; set; }
    public int? CommentCount { get; set; }
}
