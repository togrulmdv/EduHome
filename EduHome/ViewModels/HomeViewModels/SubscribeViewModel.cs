using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.HomeViewModels;

public class SubscribeViewModel
{
	[Required, DataType(DataType.EmailAddress)]
	public string Email { get; set; }
}