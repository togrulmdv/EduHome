using EduHome.Models.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.Models;

public class Course : BaseEntityAdditional
{
	public string ImageName { get; set; }
	[Required, MaxLength(100)]
	public string Name { get; set; }
	[Required, MaxLength(100)]
	public string ShortDescription { get; set; }
	[Required, MaxLength(800)]
	public string LongDescription { get; set; }
	[Required]
	public DateTime StartDate { get; set; }
	[Required]
	public string Duration { get; set; }
	[Required]
	public string ClassDuration { get; set; }
	[Required]
	public string SkillLevel { get; set; }
	[Required]
	public string Language { get; set;}
	[Required]
	public int StudentCount { get; set; }
	[Required]
	public string Assesment { get; set;}
	[Required, Column(TypeName = "decimal(6,2)")]
	public int Price { get; set;}
	public ICollection<CourseCategory> CourseCategories { get; set; }
}