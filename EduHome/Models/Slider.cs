using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Slider : BaseEntityAdditional
{
	[Required, MaxLength(100)]
	public string Title { get; set; }
	[Required, MaxLength(100)]
	public string Description { get; set; }
	public string ImageName { get; set; }
}