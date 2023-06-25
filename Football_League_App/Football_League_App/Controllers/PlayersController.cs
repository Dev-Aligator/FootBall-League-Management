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
            ViewData["MaClb"] = new SelectList(_context.Clubs, "MaClb", "MaClb");
            return View();
        }

        public IActionResult Detail()
        {
            // Create a list of Club options for the first select tag
            List<SelectListItem> clubOptions = _context.Clubs
                .Select(c => new SelectListItem { Value = c.MaClb.ToString(), Text = c.TenClb })
                .ToList();

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


        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaCt,TenCt,LoaiCt,QuocTich,Luong,ChieuCao,CanNang,ViTriChinh,ViTriPhu,NgaySinh,SoAo,ChanThuan,MaClb")] Player player)
        {
            SqlConnection con = new(connectString);
            string query = "Insert into Players values(@TenCT,@LoaiCT,@QuocTich,@Luong,@ChieuCao,@CanNang,@ViTriChinh,@ViTriPhu,@NgaySinh,@SoAo,@ChanThuan,@MaCLB)";
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
            ViewData["MaClb"] = new SelectList(_context.Clubs, "MaClb", "MaClb", player.MaClb);
            return View(player);
        }
        
        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,MaCt,TenCt,LoaiCt,QuocTich,Luong,ChieuCao,CanNang,ViTriChinh,ViTriPhu,NgaySinh,SoAo,ChanThuan,MaClb")] Player player)
        {
            if (id != player.MaCt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
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
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaClb"] = new SelectList(_context.Clubs, "MaClb", "MaClb", player.MaClb);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.MaClbNavigation)
                .FirstOrDefaultAsync(m => m.MaCt == id);
            if (player == null)
            {
                return NotFound();
            }

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
    }
}
