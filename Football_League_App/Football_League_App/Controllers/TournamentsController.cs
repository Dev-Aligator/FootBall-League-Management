using Football_League_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Football_League_App.Controllers
{
	public class TournamentsController : Controller
	{
		List<Player> list = new List<Player>();
		string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";
		readonly IWebHostEnvironment _webHostEnvironment;

		public TournamentsController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Clubs()
		{
			List<Club> list = GetClubsList();
			ViewBag.model = list;
			return View();
		}

		public IActionResult Leagues()
		{
			string currentUsername = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;

			List<League> list = GetLeaguesList(GetMaUsersFromUserName(currentUsername), true);
			ViewBag.model = list;
			return View();
		}

		public IActionResult LeaguesInfo()
		{
			string currentUsername = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;

			List<League> list = GetLeaguesList(GetMaUsersFromUserName(currentUsername), false);
			ViewBag.model = list;
			return View();
		}

		public IActionResult Players()
		{
			list = GetPlayersList();
			ViewBag.model = list;
			return View();
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

		public List<Club> GetClubsList()
		{
			List<Club> list = new();
			string query = "SELECT * FROM Clubs";
			SqlConnection con = new SqlConnection(connectString);
			con.Open();
			SqlCommand sqlCommand = new SqlCommand(query, con);
			SqlDataReader reader = sqlCommand.ExecuteReader();
			while (reader.Read())
			{
				Club club = new Club()
				{
					Id = (int)reader["ID"],
					TenClb = reader["TenCLB"].ToString(),
					DiaChi = reader["DiaChi"].ToString(),
					TenSvd = reader["TenSVD"].ToString(),
					ImgPath = reader["Img_File"].ToString(),
				};
				list.Add(club);
			}
			con.Close ();
			//logo từng đội sẽ đc phân biệt = "MãCLB hoặc TênCLB + định dạng file"
			//vd club002 -> logo file: "club002.png"
			return list;
		}

		public IActionResult AddClub(string clubName, string clubAddr, string clubStad, IFormFile clubPhoto)
		{
			AddClubToDb(clubName,clubAddr, clubStad, clubPhoto);
			return RedirectToAction("Clubs", "Tournaments");
		}

		void AddClubToDb(string clubName, string clubAddr, string clubStad, IFormFile clubPhoto)
		{
			SqlConnection con = new SqlConnection(connectString);
			SqlCommand cmd = new("Insert Into Clubs  (TenCLB, TenSVD, DiaChi, Img_File) values(@tenclub,@diachi,@tensvd,@imgpath)", con);
			con.Open();
			FlmdbContext flmdb = new();
			Club club = new();
			try
			{
				cmd.Parameters.Add("@tenclub", System.Data.SqlDbType.NVarChar).Value = clubName;
				cmd.Parameters.Add("@diachi", System.Data.SqlDbType.NVarChar).Value = clubAddr;
				cmd.Parameters.Add("@tensvd", System.Data.SqlDbType.NVarChar).Value = clubStad;

				/*if (clubPhoto != null) 
				{
					string folder = "images/";
					folder += clubName + club.ClubPhoto.FileName;
					string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
					club.ClubPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
				}*/
				//cmd.Parameters.Add("@imgpath", System.Data.SqlDbType.NVarChar).Value = club.ClubPhoto.ToString();
				//Có vấn đề trong việc thêm ảnh
				cmd.ExecuteNonQuery();
				TempData["Message"] = "Add Club To Db Success!";
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Failed To Add Club To Db, Error: " + ex.Message;
			}
			con.Close();
		}

		/*public ActionResult AddingImage(IFormFile clubLogo)
		{
			FlmdbContext db=new();
			if (clubLogo != null)
			{
				int id = int.Parse(db.Clubs.ToList().Last().MaClb.ToString());
				string filename = "images/";
				int index = clubLogo.FileName.IndexOf('.');
				filename = "Club" + id.ToString() + "." + clubLogo.FileName.Substring(index + 1);
				string path = Path.Combine(_webHostEnvironment.WebRootPath, filename);
				clubLogo.CopyToAsync(new FileStream(path, FileMode.Create));
			}
			return RedirectToAction("Club", "Tournaments");
		}*/

		public List<League> GetLeaguesList(string userId, bool is_public)
		{
			List<League> list = new();
			string query = "";
			if (GetLoaiUsersFromMaUsers(userId) == 0)
			{
				query = "SELECT * FROM League";

			}
			else if(is_public)
			{
			
				query = "SELECT * FROM League where UserId = @userId OR IsPublic = 1";
               
			}
			else
			{
                query = "SELECT * FROM League where UserId = @userId";
            }
			SqlConnection con = new SqlConnection(connectString);
			con.Open();

			try
			{
				SqlCommand sqlCommand = new SqlCommand(query, con);
				sqlCommand.Parameters.AddWithValue("@userId", userId);
				SqlDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					League league = new League()
					{
						Id = (int)reader["ID"],
						MaLeague = reader["MaLeague"].ToString(),
						LeagueName = reader["LeagueName"].ToString(),
						MaxTeams = (int)reader["MaxTeams"],
						StartDate = DateTime.Parse(reader["StartDate"].ToString()),
						EndDate = DateTime.Parse(reader["EndDate"].ToString()),
					};
					list.Add(league);
				}
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Error: " + ex.Message;
			}
			con.Close();
			return list;
		}

		public IActionResult Rules()
		{
			return View();
		}

		private string GetMaUsersFromUserName(string userName)
		{
			FlmdbContext flmDb = new FlmdbContext();
			User user = flmDb.Users.FirstOrDefault(u => u.UserName == userName);

			return user?.MaUsers;
		}

		private int GetLoaiUsersFromMaUsers(string maUsers)
		{
			FlmdbContext flmDb = new FlmdbContext();
			User user = flmDb.Users.FirstOrDefault(u => u.MaUsers == maUsers);

			return user?.LoaiUsers ?? 2;
		}

	}
}
