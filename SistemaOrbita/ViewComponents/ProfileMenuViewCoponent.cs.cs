using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ViewComponents
{
	public class ProfileMenu : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
