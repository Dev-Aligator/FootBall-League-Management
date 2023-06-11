using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Football_League_App.Models;
using Microsoft.Data.SqlClient;

namespace Football_League_App.Controllers
{
    public class PlayersController : Controller
    {
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

        // GET: Players/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["MaClb"] = new SelectList(_context.Clubs, "MaClb", "MaClb");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaCt,TenCt,LoaiCt,QuocTich,Luong,ChieuCao,CanNang,ViTriChinh,ViTriPhu,NgaySinh,SoAo,ChanThuan,MaClb")] Player player)
        {
            SqlConnection con = new("Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True");
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

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
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
