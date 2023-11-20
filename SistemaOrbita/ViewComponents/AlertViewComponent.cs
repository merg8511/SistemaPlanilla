using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ViewComponents
{
	public class AlertViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
