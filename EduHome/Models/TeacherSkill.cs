using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class TeacherSkill : BaseEntity
{
	public int TeacherId { get; set; }
	public Teacher Teacher { get; set; }
	public int SkillId { get; set; }
	public Skill Skill { get; set; }
	[Required, Range(0, 100)]
	public float Percentage { get; set; }
}