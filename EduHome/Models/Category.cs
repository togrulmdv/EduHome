using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Category : BaseEntityAdditional
{
	[Required]
	public string Name { get; set; }
	public ICollection<CourseCategory> CourseCategories { get; set; }
}