using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents;

public class SubscribeViewComponent : ViewComponent
{
	public async Task<IViewComponentResult> InvokeAsync()
	{
		return View();
	}
}
