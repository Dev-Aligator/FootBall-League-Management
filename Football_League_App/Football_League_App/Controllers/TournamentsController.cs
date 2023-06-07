using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Football_League_App.Controllers
{
	public class TournamentsController : Controller
	{
		List<Player> list = new List<Player>();
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
			return View();
		}


		public IActionResult AddPlayerIntoDatabase(Player player)
		{
			return View();
		}
		public List<Player> GetPlayersList()
		{
			List<Player> list = new List<Player>();
			string connectString = "Data Source=DESKTOP-EKAMM32;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";
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
					Luong = (int)reader["Luong"],
					ChieuCao = (int)reader["ChieuCao"],
					CanNang = (int)reader["CanNang"],
					ViTriChinh = reader["ViTriChinh"].ToString(),
					ViTriPhu = reader["ViTriPhu"].ToString(),
					NgaySinh = DateTime.Parse(reader["NgaySinh"].ToString()),
					SoAo = (int)reader["SoAo"],
					ChanThuan = reader["ChanThuan"].ToString(),
					MaClb = reader["MaCLB"].ToString(),


				};
				list.Add(player);
			}
			return list;
		}

		public IActionResult Rules()
		{
			return View();
		}

	}
}
