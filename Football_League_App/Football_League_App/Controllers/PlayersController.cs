using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Football_League_App.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Football_League_App.Controllers
{
    public class PlayersController : Controller
    {
        string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True";
        private readonly FlmdbContext _context;

        public PlayersController(FlmdbContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var flmdbContext = _context.Players;
            return View(await flmdbContext.ToListAsync());
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["MaClb"] = _context.Clubs
                .Select(c => new SelectListItem { Value = c.MaClb.ToString(), Text = c.TenClb })
                .ToList();
            return View();
        }

        public IActionResult Detail()
        {
			string currentUsername = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
            // Create a list of Club options for the first select tag
            List<SelectListItem> clubOptions = null;

			string MaUser = GetMaUsersFromUserName(currentUsername);

            if (GetLoaiUsersFromMaUsers(MaUser) == 0)
            {
				clubOptions = _context.Clubs
				.Select(c => new SelectListItem { Value = c.MaClb.ToString(), Text = c.TenClb })
				.ToList();
			}
            else 
            {
				clubOptions = _context.Clubs
				.Where(c => c.UserId == MaUser)
				.Select(c => new SelectListItem { Value = c.MaClb.ToString(), Text = c.TenClb })
				.ToList();
			}
			

            // Add the select tag options to the ViewBag
            ViewBag.MaClb = clubOptions;

            // Create a list of Player options for the second select tag
            List<SelectListItem> playerOptions = new List<SelectListItem>();

            if (clubOptions.Count > 0)
            {
                // Get the selected club value (default to the first option)
                string selectedClub = Convert.ToString(clubOptions.FirstOrDefault().Value);

                // Filter the players by the selected club
                playerOptions = _context.Players
                    .Where(p => p.MaClb == selectedClub)
                    .Select(p => new SelectListItem { Value = p.TenCt, Text = p.TenCt })
                    .ToList();
            }

            ViewBag.TenCt = playerOptions;

            return View();
        }
        public JsonResult GetPlayers(string maClb)
        {
            var players = _context.Players
                .Where(p => p.MaClb == maClb)
                .Select(p => new SelectListItem { Value = p.TenCt, Text = p.TenCt });
            return Json(players);
        }

        public ActionResult GetPlayerDetails(string maClb, string tenCt)
        {
            //Retrieve the player details based on the club code and player name
            var player = GetPlayerDetailsFromDatabase(maClb, tenCt);

            //Serialize the player object to JSON and return it
            return Content(JsonConvert.SerializeObject(player), "application/json");
        }

        private Player GetPlayerDetailsFromDatabase(string maClb, string tenCt)
        {
            string querry = "SELECT * FROM Players Where MaCLB = @maClb and TenCT=@tenCt";
            SqlConnection con = new SqlConnection(connectString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand(querry, con);
            try
            {
                sqlCommand.Parameters.AddWithValue("maClb", maClb);
                sqlCommand.Parameters.AddWithValue("tenCt", tenCt);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                //display error
            }
            SqlDataReader reader = sqlCommand.ExecuteReader();
            Player player = new Player();
            while (reader.Read())
            {
                player = new Player()
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
               
            }
            con.Close(); //LUÔN NHỚ ĐÓNG KẾT NỐI
            return player;
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            ViewBag.MaClb = new SelectList(_context.Clubs, "MaClb", "TenClb", player.MaClb);
            ViewBag.TenCt = new SelectList(_context.Players, "MaCt", "TenCt", player.TenCt);

            return View(player);
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaCt,TenCt,LoaiCt,QuocTich,Luong,ChieuCao,CanNang,ViTriChinh,ViTriPhu,NgaySinh,SoAo,ChanThuan,MaClb")] Player player)
        {
            SqlConnection con = new(connectString);
            string query = "Insert into Players values(@TenCT,@LoaiCT,@QuocTich,@ChieuCao,@CanNang,@ViTriChinh,@ViTriPhu,@NgaySinh,@SoAo,@ChanThuan,@MaCLB,@Luong)";
            SqlCommand cmd = new(query, con);
            con.Open();
            try
            {
                cmd.Parameters.AddWithValue("TenCT", player.TenCt);
                cmd.Parameters.AddWithValue("LoaiCT", player.LoaiCt);
                cmd.Parameters.AddWithValue("QuocTich", player.QuocTich);
                cmd.Parameters.AddWithValue("Luong", player.Luong);
                cmd.Parameters.AddWithValue("ChieuCao", player.ChieuCao);
                cmd.Parameters.AddWithValue("CanNang", player.CanNang);
                cmd.Parameters.AddWithValue("ViTriChinh", player.ViTriChinh);
                cmd.Parameters.AddWithValue("ViTriPhu", player.ViTriPhu);
                cmd.Parameters.AddWithValue("NgaySinh", player.NgaySinh);
                cmd.Parameters.AddWithValue("SoAo", player.SoAo);
                cmd.Parameters.AddWithValue("ChanThuan", player.ChanThuan);
                cmd.Parameters.AddWithValue("MaCLB", player.MaClb);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                //display error
            }
            con.Close();
			string currentUsername = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;


			List<Club> allClubs = GetClubsByUsers(GetMaUsersFromUserName(currentUsername));
			SelectList clubList = new SelectList(allClubs, "MaClb", "MaClb");
            ViewData["MaClb"] = clubList;
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string MaCt, [Bind("Id, MaCt, TenCt,LoaiCt,QuocTich,Luong,ChieuCao,CanNang,ViTriChinh,ViTriPhu,NgaySinh,SoAo,ChanThuan,MaClb")] Player player)
        {
            if (MaCt != player.MaCt)
            {
                return NotFound();
            }

            try
            {
                // Retrieve the existing player from the database based on MaCt
                var existingPlayer = await _context.Players.FindAsync(MaCt);

                if (existingPlayer != null)
                {
                    // Update the existing player with the new details
                    existingPlayer.TenCt = player.TenCt;
                    existingPlayer.LoaiCt = player.LoaiCt;
                    existingPlayer.QuocTich = player.QuocTich;
                    existingPlayer.Luong = player.Luong;
                    existingPlayer.ChieuCao = player.ChieuCao;
                    existingPlayer.CanNang = player.CanNang;
                    existingPlayer.ViTriChinh = player.ViTriChinh;
                    existingPlayer.ViTriPhu = player.ViTriPhu;
                    existingPlayer.NgaySinh = player.NgaySinh;
                    existingPlayer.SoAo = player.SoAo;
                    existingPlayer.ChanThuan = player.ChanThuan;
                    existingPlayer.MaClb = player.MaClb;

                    // Save the changes to the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Players", "Tournaments");
                }

                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(player.MaCt))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
<<<<<<< Updated upstream
                return RedirectToAction(nameof(Index));
            }
			string currentUsername = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;


            List<Club> allClubs = GetClubsByUsers(GetMaUsersFromUserName(currentUsername));
			SelectList clubList = new SelectList(allClubs, "MaClb", "MaClb");

            ViewData["MaClb"] = clubList;
=======
            }

            ViewData["MaClb"] = new SelectList(_context.Clubs, "MaClb", "MaClb", player.MaClb);
            ViewData["TenCt"] = new SelectList(_context.Players, "TenCt", "TenCt", player.TenCt);
>>>>>>> Stashed changes
            return View(player);
        }
        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Players == null)
            {
                return Problem("Entity set 'FlmdbContext.Players'  is null.");
            }
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(string id)
        {
            return (_context.Players?.Any(e => e.MaCt == id)).GetValueOrDefault();
        }


		public List<Club> GetClubsByUsers(string userId)
		{
			List<Club> clubs = new List<Club>();

			using (SqlConnection con = new SqlConnection(connectString))
			{
                string query = "";
                if (GetLoaiUsersFromMaUsers(userId) == 0)
                {
					query = "SELECT MaClb FROM Clubs";
				}
                else
                {
					query = "SELECT MaClb FROM Clubs WHERE UserId = @userId";

				}


				SqlCommand sqlCommand = new SqlCommand(query, con);
				sqlCommand.Parameters.AddWithValue("@userId", userId);
				con.Open();

				SqlDataReader reader = sqlCommand.ExecuteReader();

				while (reader.Read())
				{
					string maClb = reader["MaClb"].ToString();

					Club club = new Club
					{
						MaClb = maClb,
						// Populate other properties if needed
					};

					clubs.Add(club);
				}

				con.Close();
			}

			return clubs;
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
