using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.AuthViewModels;

public class ForgotPasswordViewModel
{
	[Required, DataType(DataType.EmailAddress)]
	public string Email { get; set; }
}