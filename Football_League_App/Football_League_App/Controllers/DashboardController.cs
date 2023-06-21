using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Football_League_App.Controllers
{
	[Authorize] //Chỉ được vào trang Dashboard nếu đã Login THÀNH CÔNG
    public class DashboardController : Controller
    {
		string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";

		public IActionResult Index()
        {
            return View();
        }
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Access", "Login");
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
				CreateAccountForAllTeams(txtMaxTeams);
				return RedirectToAction("MainView", "Dashboard");
			}
			return RedirectToAction("MainView", "Dashboard");
		}

		private bool CreateTournament(string leagueName, int txtMaxTeams, string txtStartDate, string txtEndDate)
		{
			SqlConnection con = new("Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True");
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

		private void CreateAccountForAllTeams(int para)
		{
			SqlConnection con = new("Data Source=.\\sqlexpress;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True");
			string query = "Set Dateformat dmy\n Insert into Users values(@userName,@password,@phone,@email,1)";
			SqlCommand cmd = new(query, con);
			con.Open();
			try
			{
				for (int i = 0; i < para; i++)
				{
					string userID = "Users" + i.ToString();
					cmd.Parameters.AddWithValue("userName", userID);
					cmd.Parameters.AddWithValue("password", 1);
					cmd.Parameters.AddWithValue("phone", "");
					cmd.Parameters.AddWithValue("email", "");
					cmd.ExecuteNonQuery();
					cmd.Parameters.Clear();
				}
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Error: " + ex.Message;
			}
			con.Close();
			//Hàm này sẽ tạo tài khoản mặc định cho tất cả các CLB 
		}
	}
}
