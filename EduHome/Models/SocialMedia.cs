using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class SocialMedia : BaseEntityAdditional
{
	[Required,MaxLength(100)]
	public string URL { get; set; }
	[Required, MaxLength(100)]
	public string IconName { get; set; }
	public int TeacherId { get; set; }
	public Teacher Teacher { get; set; }

}