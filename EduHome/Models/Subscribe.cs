using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Subscribe : BaseEntityAdditional
{
	[Required, DataType(DataType.EmailAddress)]
	public string Email { get; set; }
	public bool IsSubscribed { get; set; }
}