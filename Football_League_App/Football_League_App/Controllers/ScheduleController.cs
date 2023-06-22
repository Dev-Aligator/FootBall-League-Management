using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace Football_League_App.Controllers
{
    public class ScheduleController : Controller
    {
        string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";

        public IActionResult Leagues()
        {
            List<League> list = GetLeaguesList();
            ViewBag.model = list;
            return View();
        }

        public IActionResult LeagueSchedule(string leagueId)
        {
            List<Match> matches = GetMatchesInLeague(leagueId);
            return View(matches);
        }
        public void MatchScheduler(string leagueId)
        {
            List<Match> matches = GenerateMatchSchedule(leagueId);
            using (SqlConnection con = new SqlConnection(connectString))
            {
                con.Open();

                foreach (Match match in matches)
                {
                    string query = "INSERT INTO Matchs (MaDoiNha, MaDoiKhach, SoBanThangDoiNha, SoBanThangDoiKhach, MatchDate, LeagueId) " +
                                   "VALUES (@MaDoiNha, @MaDoiKhach, @SoBanThangDoiNha, @SoBanThangDoiKhach, @MatchDate, @leagueId)";

                    SqlCommand sqlCommand = new SqlCommand(query, con);
                    sqlCommand.Parameters.AddWithValue("@MaDoiNha", match.MaDoiNhaNavigation.MaClb);
                    sqlCommand.Parameters.AddWithValue("@MaDoiKhach", match.MaDoiKhachNavigation.MaClb);
                    sqlCommand.Parameters.AddWithValue("@SoBanThangDoiNha", 0);
                    sqlCommand.Parameters.AddWithValue("@SoBanThangDoiKhach", 0);
                    sqlCommand.Parameters.AddWithValue("@MatchDate", match.MatchDate);
                    sqlCommand.Parameters.AddWithValue("@leagueId", leagueId);


                    sqlCommand.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        private List<Match> GenerateMatchSchedule(string leagueId)
        {

            List<ClubInLeague> clubs = GetClubsInLeague(leagueId);
            string query = "select StartDate, EndDate " +
                           "from League " +
                           "where MaLeague = @LeagueId ";

            SqlConnection con = new SqlConnection(connectString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@LeagueId", leagueId);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<Match> matchSchedule = new List<Match>();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            while (reader.Read())
            {
                startDate = DateTime.Parse(reader["StartDate"].ToString());
                endDate = DateTime.Parse(reader["EndDate"].ToString());

            }

            con.Close();

            foreach (ClubInLeague homeTeam in clubs)
            {
                foreach (ClubInLeague awayTeam in clubs)
                {
                    if (homeTeam != awayTeam)
                    {
                        // Create a match between homeTeam and awayTeam
                        Match match = new Match
                        {
                            MaDoiNhaNavigation = homeTeam.Club,
                            MaDoiKhachNavigation = awayTeam.Club,
                            MatchDate = CalculateMatchDate(clubs, startDate, endDate, matchSchedule.Count + 1)
                        };

                        matchSchedule.Add(match);
                    }
                }
            }


            return matchSchedule;
        }

        private DateTime CalculateMatchDate(List<ClubInLeague> clubs, DateTime leagueStartDate, DateTime leagueEndDate, int matchIndex)
        {
            // Calculate the match date based on the league start and end dates
            // You can implement your own logic here to determine the match date
            // For example, evenly distribute the matches across the league duration

            // Sample logic: Assuming matches are played on weekends, distribute them equally
            TimeSpan duration = leagueEndDate - leagueStartDate;
            int totalMatches = clubs.Count * (clubs.Count - 1); // Each team plays against every other team twice
            int daysBetweenMatches = (int)Math.Ceiling(duration.TotalDays / totalMatches);

            // Calculate the match date by adding the match index multiplied by daysBetweenMatches to the league start date
            DateTime matchDate = leagueStartDate.AddDays((matchIndex - 1) * daysBetweenMatches);

            return matchDate;
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

        private List<Match> GetMatchesInLeague(string leagueId)
        {
            // Retrieve all matches in the specified league from the database
            string query = "SELECT * FROM Matchs WHERE LeagueId = @LeagueId ORDER BY MatchDate";
            SqlConnection con = new SqlConnection(connectString);
            con.Open();
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@LeagueId", leagueId);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<Match> matches = new List<Match>();
            while (reader.Read())
            {
                Match match = new Match
                {
                    Id = (int)reader["ID"],
                    MaTd = reader["MaTD"].ToString(),
                    MaDoiNha = reader["MaDoiNha"].ToString(),
                    MaDoiKhach = reader["MaDoiKhach"].ToString(),
                    SoBanThangDoiNha = reader.IsDBNull(reader.GetOrdinal("SoBanThangDoiNha")) ? null : (int?)reader["SoBanThangDoiNha"],
                    SoBanThangDoiKhach = reader.IsDBNull(reader.GetOrdinal("SoBanThangDoiKhach")) ? null : (int?)reader["SoBanThangDoiKhach"],
                    MatchDate = reader.IsDBNull(reader.GetOrdinal("MatchDate")) ? DateTime.MinValue : (DateTime)reader["MatchDate"],
                    // Populate other properties if needed
                };

                Club homeClub = GetClubById(match.MaDoiNha);
                match.MaDoiNhaNavigation = homeClub;

                // Retrieve club information for away team (MaDoiKhach)
                Club awayClub = GetClubById(match.MaDoiKhach);
                match.MaDoiKhachNavigation = awayClub;

                matches.Add(match);
            }

            con.Close();
            return matches;
        }

        public Club GetClubById(string clubId)
        {
            string query = "SELECT * FROM Clubs WHERE MaCLB = @ClubId";

            using (SqlConnection con = new SqlConnection(connectString))
            {
                con.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    sqlCommand.Parameters.AddWithValue("@ClubId", clubId);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Club club = new Club
                            {
                                MaClb = reader["MaCLB"].ToString(),
                                TenClb = reader["TenCLB"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                TenSvd = reader["TenSVD"].ToString(),
                                ImgPath = reader["Img_File"].ToString(),
                                // Populate other club properties from the reader
                            };

                            return club;
                        }
                    }
                }
            }

            return null; // Club not found
        }
    }
}
