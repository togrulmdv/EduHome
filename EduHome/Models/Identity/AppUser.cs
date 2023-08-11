using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models.Identity;

public class AppUser : IdentityUser
{
	[Required, MaxLength(747)]
	public string Name { get; set; }
	[Required, MaxLength(747)]
	public string Surname { get; set; }
	public bool IsActive { get; set; }
}