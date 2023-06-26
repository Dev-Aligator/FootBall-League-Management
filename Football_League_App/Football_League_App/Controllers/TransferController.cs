using Microsoft.AspNetCore.Mvc;

namespace Football_League_App.Controllers
{
    public class TransferController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TransferPlayers() 
        {
            return View();
        }
    }
}
