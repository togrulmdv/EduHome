using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.AuthViewModels;

public class ResetPasswordViewModel
{
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string NewPasswordConfirm { get; set;}
}