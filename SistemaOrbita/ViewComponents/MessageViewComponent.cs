using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ViewComponents
{
	public class MessageViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
