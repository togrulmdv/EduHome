using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Blog : BaseEntityAdditional
{
	public string ImageName { get; set; }
	[Required]
	public DateTime Date { get; set; }
	[Required, MaxLength(100)]
	public string Name { get; set; }
	[Required, MaxLength(800)]
	public string Description { get; set; }
	public int? CommentCount { get; set; }
}