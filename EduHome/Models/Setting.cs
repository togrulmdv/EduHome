using EduHome.Models.Common;

namespace EduHome.Models;

public class Setting : BaseEntity
{
	public string Key { get; set; }
	public string Value { get; set; }
}
