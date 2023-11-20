using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ViewComponents
{
	public class SearchViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
