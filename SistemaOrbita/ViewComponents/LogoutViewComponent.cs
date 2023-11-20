using Microsoft.AspNetCore.Mvc;

namespace SistemaOrbita.ViewComponents
{
    public class LogoutViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
