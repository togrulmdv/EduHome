using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.AuthViewModels;

public class TwoFactorViewModel
{
	[Required, DataType(DataType.Text)]
	public string TwoFactorCode { get; set; }
	public bool RememberMe { get; set; }
}