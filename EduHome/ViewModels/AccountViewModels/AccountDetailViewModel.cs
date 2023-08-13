namespace EduHome.ViewModels.AccountViewModels;

public class AccountDetailViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
}