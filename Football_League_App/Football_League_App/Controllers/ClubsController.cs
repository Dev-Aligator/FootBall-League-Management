using Microsoft.AspNetCore.Mvc;

namespace Football_League_App.Controllers
{
	public class ClubsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Information()
		{
			return View();
		}

		public IActionResult Players()
		{
			return View();
		}
	}
}
