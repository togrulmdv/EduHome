using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.AccountViewModels;

public class RegisterViewModel
{
	[Required, MaxLength(747)]
	public string Name { get; set; }
	[Required, MaxLength(747)]
	public string Surname { get; set; }
	[Required, MaxLength(100)]
	public string UserName { get; set; }
	[Required, MaxLength(100), DataType(DataType.EmailAddress)]
	public string Email { get; set; }
	[Required, MinLength(8), DataType(DataType.Password)]
	public string Password { get; set; }
	[Required, MinLength(8), DataType(DataType.Password), Compare(nameof(Password))]
	public string PasswordConfirm { get; set; }

}
