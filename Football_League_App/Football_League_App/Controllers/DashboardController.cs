using Microsoft.AspNetCore.Mvc;

namespace Football_League_App.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MainView()
        {
            return View(); //MainView.cshtml
        }
    }
}
