using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.EventViewModels;

public class EventCardViewModel
{
	public int Id { get; set; }
	public string ImageName { get; set; }
	public DateTime Date { get; set; }
	public string Name { get; set; }
	public string Time { get; set; }
	public string Venue { get; set; }
}
