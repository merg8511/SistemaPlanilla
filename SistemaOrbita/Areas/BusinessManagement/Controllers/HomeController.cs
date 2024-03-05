using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaOrbita.Model.ViewModels;
using System.Diagnostics;

namespace SistemaOrbita.Areas.BusinessManagement.Controllers
{
    [Area("BusinessManagement")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/BusinessManagement/Home/Error/{code:int}")]
        public IActionResult Error(int? code)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = code,
                Message = code switch
                {
                    404 => "The requested page was not found.",
                    403 => "You do not have permission to access this page.",
                    500 => "Internal server error.",
                    _ => "An unexpected error occurred."
                }
            };
            return View(model);
        }
    }
}