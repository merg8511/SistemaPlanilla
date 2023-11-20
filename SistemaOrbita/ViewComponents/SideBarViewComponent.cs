using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ViewComponents
{
	public class SideBarViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
