using EduHome.Models;
using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.EventViewModels;

public class EventDetailViewModel
{
	public string ImageName { get; set; }
	[Required]
	public DateTime Date { get; set; }
	[Required, MaxLength(100)]
	public string Name { get; set; }
	[Required, MaxLength(800)]
	public string Description { get; set; }
	[Required]
	public string Time { get; set; }
	[Required]
	public string Venue { get; set; }
	public ICollection<Speaker> Speakers { get; set; }
}
