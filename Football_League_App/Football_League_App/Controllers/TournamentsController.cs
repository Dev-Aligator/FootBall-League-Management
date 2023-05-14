using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Football_League_App.Controllers
{
	public class TournamentsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Clubs()
		{
			return View();
		}

		public IActionResult Players()
		{
			return View();
		}

		public IActionResult Rules()
		{
			return View();
		}

	}
}
