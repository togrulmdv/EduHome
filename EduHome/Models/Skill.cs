using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Skill : BaseEntityAdditional
{
	[Required, MaxLength(100)]
	public string Name { get; set; }
	public ICollection<TeacherSkill> TeacherSkills { get; set; }
}