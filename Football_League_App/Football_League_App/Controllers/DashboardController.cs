using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Globalization;

namespace Football_League_App.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult MainView()
        {
            return View();
        }
        [HttpPost]
		public IActionResult MainView(string leagueName, int txtMaxTeams, string txtStartDate, string txtEndDate)
		{
			if (CreateTournament(leagueName, txtMaxTeams, txtStartDate, txtEndDate))
			{
				TempData["Message"] = "Create A Tournamanent Successfully!";
				return RedirectToAction("MainView", "Dashboard");
			}
			return RedirectToAction("MainView", "Dashboard");
		}

		bool CreateTournament(string leagueName, int txtMaxTeams, string txtStartDate, string txtEndDate)
		{
			SqlConnection con = new("Data Source=.\\sqlexpress;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True");
			string query = "Set Dateformat dmy\n Insert into League values(@leagueName,@MaxTeams,@startDate,@endDate)";
			SqlCommand cmd = new(query, con);
			con.Open();
			try
			{
				cmd.Parameters.AddWithValue("leagueName", leagueName);
				cmd.Parameters.AddWithValue("MaxTeams", txtMaxTeams);
				DateTime dt = DateTime.Parse(txtStartDate);
				cmd.Parameters.AddWithValue("startDate", dt);
			    dt = DateTime.Parse(txtEndDate);
				cmd.Parameters.AddWithValue("endDate", dt);
				cmd.ExecuteNonQuery();
				return true;
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Error: " + ex.Message;
			}
			con.Close();
			return false;
			//Hàm này để check xem tạo giải đấu có thành công hay không
		}
	}
}
