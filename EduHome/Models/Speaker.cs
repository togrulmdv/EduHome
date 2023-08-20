using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Speaker : BaseEntityAdditional
{
	[Required, MaxLength(100)] 
	public string Name { get; set; }
	[Required, MaxLength(30)]
	public string Role { get; set; }
	[Required, MaxLength(30)]
	public string Company { get; set; }
	public string ImageName { get; set; }
	public ICollection<EventSpeaker> EventSpeakers { get; set; }
}
