using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Football_League_App.Controllers
{
	public class TournamentsController : Controller
	{
		List<Player> list = new List<Player>();
		string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";

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
			list = GetPlayersList();
			ViewBag.model = list;
			return View();
		}

		public IActionResult AddPlayer()
		{
			//return RedirectToAction("Players", "Tournaments");
			return View();
		}

		public IActionResult AddPlayerIntoDatabase() 
		{
			AddingPlayersToDb();

			return RedirectToAction("Players", "Tournaments");
		}

		void AddingPlayersToDb()
		{
			try
			{
				SqlConnection con = new SqlConnection(connectString);
				con.Open();
				SqlCommand cmd = new("Set Dateformat dmy\nINSERT INTO Players ([@TenCT], [@LoaiCT], [@Luong], [@ChieuCao], [@CanNang], [@ViTriChinh], [@ViTriPhu], [@NgaySinh], [@SoAo], [@ChanThuan])", con);
			    //cmd.Parameters.Add("@TenCT", System.Data.SqlDbType.VarChar).Value = txtTenCT.Text;
			}
			catch (Exception ex) 
			{

			}
		}
		public List<Player> GetPlayersList()
		{
			List<Player> list = new List<Player>();
			string querry = "SELECT * FROM Players";
			SqlConnection con = new SqlConnection(connectString);
			con.Open();
			SqlCommand sqlCommand = new SqlCommand(querry, con);
			SqlDataReader reader = sqlCommand.ExecuteReader();
			while (reader.Read())
			{
				Player player = new Player()
				{
					Id = (int)reader["ID"],
					MaCt = reader["MaCT"].ToString(),
					TenCt = reader["TenCT"].ToString(),
					LoaiCt = (int)reader["LoaiCT"],
					QuocTich = reader["QuocTich"].ToString(),
					ChieuCao = (int)reader["ChieuCao"],
					CanNang = (int)reader["CanNang"],
					ViTriChinh = reader["ViTriChinh"].ToString(),
					ViTriPhu = reader["ViTriPhu"].ToString(),
					NgaySinh = DateTime.Parse(reader["NgaySinh"].ToString()),
					SoAo = (int)reader["SoAo"],
					ChanThuan = reader["ChanThuan"].ToString(),
					MaClb = reader["MaCLB"].ToString(),
					Luong = (int)reader["Luong"],
				};
				list.Add(player);
			}
			con.Close(); //LUÔN NHỚ ĐÓNG KẾT NỐI
			return list;
		}

		public IActionResult Rules()
		{
			return View();
		}

	}
}
