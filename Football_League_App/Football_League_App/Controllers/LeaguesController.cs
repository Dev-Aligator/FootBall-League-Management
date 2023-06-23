using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Football_League_App.Controllers
{
    public class LeaguesController : Controller
    {
        string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";

       
        public IActionResult Editing(string leagueId)
        {
            League league = GetLeagueById(leagueId);
            return View(league);
        }

        [HttpPost]
        public IActionResult Editing(League league)
        {

            string query = "UPDATE League SET LeagueName = @LeagueName, MaxTeams = @MaxTeams, StartDate = @StartDate, EndDate = @EndDate WHERE MaLeague = @MaLeague";

            using (SqlConnection con = new SqlConnection(connectString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LeagueName", league.LeagueName);
                cmd.Parameters.AddWithValue("@MaxTeams", league.MaxTeams);
                cmd.Parameters.AddWithValue("@StartDate", league.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", league.EndDate);
                cmd.Parameters.AddWithValue("@MaLeague", league.MaLeague);

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("LeaguesInfo", "Tournaments");


           
            // Perform the necessary operations to update the league
            // You can access the league properties from the submitted form data

            // Redirect to a success page or another action



        


        }


        public IActionResult ModifyClubs(string leagueId)
        {
            List<ClubInLeague> clubs = GetClubsInLeague(leagueId);
            League league = GetLeagueById(leagueId);
            ViewBag.leagueName = league.LeagueName;
            ViewBag.leagueId = leagueId;

            List<Club> allClubs = GetClubsFromDatabase(); // Fetch clubs from the database using your SQL query

            SelectList clubList = new SelectList(allClubs, "MaClb", "MaClb");

            ViewData["MaClb"] = clubList;
            return View(clubs);
        }

        public IActionResult RemoveClubFromLeague(string clubId, string leagueId)
        {
            string query = "DELETE FROM ClubInLeague WHERE MaCLB = @clubId AND MaLeague= @leagueId";
            SqlConnection con = new SqlConnection(connectString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@clubId", clubId);
            sqlCommand.Parameters.AddWithValue("@leagueId", leagueId);

            SqlDataReader reader = sqlCommand.ExecuteReader();
           
         
            con.Close();
            return RedirectToAction("ModifyClubs", new { leagueId = leagueId });

        }


        public IActionResult AddClubToLeague(string selectedClub, string leagueId)
        {
            string query = "INSERT INTO ClubInLeague VALUES (@MaClb, @MaLeague, 0 , 0 , 0 ,0 ,0)";

            SqlConnection con = new SqlConnection(connectString);
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@MaClb", selectedClub);
            command.Parameters.AddWithValue("@MaLeague", leagueId);

            con.Open();
            command.ExecuteNonQuery();
            con.Close();


            return RedirectToAction("ModifyClubs", new { leagueId = leagueId });
        }

        private List<ClubInLeague> GetClubsInLeague(string leagueId)
        {
            string query = "SELECT c.*, cil.MatchesPlayed, cil.Points, cil.Wins, cil.Losses, cil.Draws " +
                           "FROM Clubs c " +
                           "INNER JOIN ClubInLeague cil ON c.MaCLB = cil.MaCLB " +
                           "WHERE cil.MaLeague = @LeagueId";

            SqlConnection con = new SqlConnection(connectString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@LeagueId", leagueId);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<ClubInLeague> clubsInLeague = new List<ClubInLeague>();
            while (reader.Read())
            {
                Club club = new Club()
                {
                    MaClb = reader["MaCLB"].ToString(),
                    TenClb = reader["TenCLB"].ToString(),
                    DiaChi = reader["DiaChi"].ToString(),
                    TenSvd = reader["TenSVD"].ToString(),
                    ImgPath = reader["Img_File"].ToString()
                };

                ClubInLeague clubInLeague = new ClubInLeague()
                {
                    ClubId = club.MaClb,
                    Club = club,
                    MatchesPlayed = (int)reader["MatchesPlayed"],
                    Points = (int)reader["Points"],
                    Wins = (int)reader["Wins"],
                    Losses = (int)reader["Losses"],
                    Draws = (int)reader["Draws"]
                };

                clubsInLeague.Add(clubInLeague);
            }

            con.Close();
            clubsInLeague = clubsInLeague.OrderByDescending(c => c.Points).ToList();

            return clubsInLeague;
        }

        public League GetLeagueById(string leagueId)
        {
            using (SqlConnection con = new SqlConnection(connectString))
            {
                string query = "SELECT * FROM League WHERE MaLeague = @LeagueId";

                con.Open();
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.Parameters.AddWithValue("@LeagueId", leagueId);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    League league = new League
                    {
                        MaLeague = reader["MaLeague"].ToString(),
                        LeagueName = reader["LeagueName"].ToString(),
                        MaxTeams = (int)reader["MaxTeams"],
                        StartDate = DateTime.Parse(reader["StartDate"].ToString()),
                        EndDate = DateTime.Parse(reader["EndDate"].ToString()),

                    // Populate other properties of the League object as needed
                };

                    return league;
                }
            }

            return null; // If league is not found
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
            con.Close();
            //logo từng đội sẽ đc phân biệt = "MãCLB + định dạng file"
            //vd club002 -> logo file: "club002.png"
            return list;
        }

        public List<League> GetLeaguesList()
        {
            List<League> list = new();
            string query = "SELECT * FROM League";
            SqlConnection con = new SqlConnection(connectString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand(query, con);
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
            con.Close();
            return list;
        }


        public List<Club> GetClubsFromDatabase()
        {
            List<Club> clubs = new List<Club>();

            using (SqlConnection con = new SqlConnection(connectString))
            {
                string query = "SELECT MaClb FROM Clubs";

                SqlCommand sqlCommand = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    string maClb = reader["MaClb"].ToString();

                    Club club = new Club
                    {
                        MaClb = maClb
                        // Populate other properties if needed
                    };

                    clubs.Add(club);
                }

                con.Close();
            }

            return clubs;
        }



    }
}
