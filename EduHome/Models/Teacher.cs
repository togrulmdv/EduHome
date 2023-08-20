using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Teacher : BaseEntityAdditional
{
	public string ImageName { get; set; }
	[Required, MaxLength(100)]
	public string Name { get; set; }
	[Required, MaxLength(100)]
	public string Role { get; set; }
	[Required, MaxLength(800)]
	public string Description { get; set; }
	[Required, MaxLength(100)]
	public string Degree { get; set; }
	[Required, MaxLength(100)]
	public string Experience { get; set; }
	[Required, MaxLength(100)]
	public string Hobbies { get; set; }
	[Required, MaxLength(100)]
	public string Faculty { get; set; }
	[Required, DataType(DataType.EmailAddress)]
	public string Email { get; set; }
	[Required, DataType(DataType.PhoneNumber)]
	public string PhoneNumber { get; set; }
	[Required, MaxLength(100)]
	public string Skype { get; set; }
	public ICollection<TeacherSkill> TeacherSkills { get; set; }
	public ICollection<SocialMedia> SocialMedias { get; set; }
}