using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static System.Reflection.Metadata.BlobBuilder;

namespace Football_League_App.Controllers
{
    public class ScheduleController : Controller
    {

        string connectString = "Data Source=.\\SQLEXPRESS;Initial Catalog=FLMDB;Integrated Security=True; MultipleActiveResultSets = True; Encrypt = False; TrustServerCertificate = True";

        public IActionResult Leagues()
        {
			string currentUsername = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;

			List<League> list = GetLeaguesList(GetMaUsersFromUserName(currentUsername));


			ViewBag.model = list;
            ViewBag.user = GetMaUsersFromUserName(currentUsername);
            return View();
        }

        public IActionResult LeagueSchedule(string leagueId)
        {
            List<Match> matches = GetMatchesInLeague(leagueId);
            ViewBag.LeagueId = leagueId;
            string leagueName = GetLeagueName(leagueId);
            ViewBag.LeagueName = leagueName;
            return View(matches);
        }
        public IActionResult MatchScheduler(string leagueId)
        {

            // Delete existing matches for the league
            using (SqlConnection con = new SqlConnection(connectString))
            {
                con.Open();

                string deleteQuery = "DELETE FROM MatchDetails WHERE MaTD IN (SELECT MaTD FROM Matchs WHERE LeagueId = @leagueId);" + "DELETE FROM Matchs WHERE LeagueId = @leagueId";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, con);
                deleteCommand.Parameters.AddWithValue("@leagueId", leagueId);
                deleteCommand.ExecuteNonQuery();

                con.Close();
            }

            List<Match> matches = GenerateMatchSchedule(leagueId);
            using (SqlConnection con = new SqlConnection(connectString))
            {
                con.Open();

                foreach (Match match in matches)
                {
                    string query = "INSERT INTO Matchs (MaDoiNha, MaDoiKhach, SoBanThangDoiNha, SoBanThangDoiKhach, MatchDate, LeagueId) " +
                                   "VALUES (@MaDoiNha, @MaDoiKhach, @SoBanThangDoiNha, @SoBanThangDoiKhach, @MatchDate, @leagueId);" +
								   "SELECT SCOPE_IDENTITY();";
                                   

                                                    

                    SqlCommand sqlCommand = new SqlCommand(query, con);
                    sqlCommand.Parameters.AddWithValue("@MaDoiNha", match.MaDoiNhaNavigation.MaClb);
                    sqlCommand.Parameters.AddWithValue("@MaDoiKhach", match.MaDoiKhachNavigation.MaClb);
                    sqlCommand.Parameters.AddWithValue("@SoBanThangDoiNha", 0);
                    sqlCommand.Parameters.AddWithValue("@SoBanThangDoiKhach", 0);
                    sqlCommand.Parameters.AddWithValue("@MatchDate", match.MatchDate);
                    sqlCommand.Parameters.AddWithValue("@leagueId", leagueId);


					int matchId = Convert.ToInt32(sqlCommand.ExecuteScalar());
					string maTD = "Match" + matchId.ToString("D3");
					string matchDetailquery = "INSERT INTO MatchDetails (MaTd) values (@matchId) ";
					SqlCommand matchDetailCommand = new SqlCommand(matchDetailquery, con);
					matchDetailCommand.Parameters.AddWithValue("@matchId", maTD);

					matchDetailCommand.ExecuteNonQuery();

				}

				con.Close();
            }

            return RedirectToAction("LeagueSchedule", new { leagueId = leagueId });
        }


        public IActionResult MatchDetails(string matchId)
        {
            MatchDetail matchDetail = null;
            string leagueName = null;

            using (SqlConnection con = new SqlConnection(connectString))
            {
                string query = "SELECT md.*, leagueId FROM MatchDetails md, Matchs m WHERE md.MaTD = m.MaTD AND md.MaTD =  @MatchId";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.Parameters.AddWithValue("@MatchId", matchId);
                con.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    matchDetail = new MatchDetail
                    {
                        MaTd = reader["MaTD"].ToString(),
						KiemSoatBongDoiNha = reader["KiemSoatBongDoiNha"] != DBNull.Value ? (int)reader["KiemSoatBongDoiNha"] : 50,
						KiemSoatBongDoiKhach = reader["KiemSoatBongDoiKhach"] != DBNull.Value ? (int)reader["KiemSoatBongDoiKhach"] : 50,
						SoCuSutDoiNha = reader["SoCuSutDoiNha"] != DBNull.Value ? (int)reader["SoCuSutDoiNha"] : 0,
						SoCuSutDoiKhach = reader["SoCuSutDoiKhach"] != DBNull.Value ? (int)reader["SoCuSutDoiKhach"] : 0,
						SoDuongChuyenDoiNha = reader["SoDuongChuyenDoiNha"] != DBNull.Value ? (int)reader["SoDuongChuyenDoiNha"] : 0,
						SoDuongChuyenDoiKhach = reader["SoDuongChuyenDoiKhach"] != DBNull.Value ? (int)reader["SoDuongChuyenDoiKhach"] : 0,
						SoLanPhamLoiDoiNha = reader["SoLanPhamLoiDoiNha"] != DBNull.Value ? (int)reader["SoLanPhamLoiDoiNha"] : 0,
						SoLanPhamLoiDoiKhach = reader["SoLanPhamLoiDoiKhach"] != DBNull.Value ? (int)reader["SoLanPhamLoiDoiKhach"] : 0,
						SoTheVangDoiNha = reader["SoTheVangDoiNha"] != DBNull.Value ? (int)reader["SoTheVangDoiNha"] : 0,
						SoTheVangDoiKhach = reader["SoTheVangDoiKhach"] != DBNull.Value ? (int)reader["SoTheVangDoiKhach"] : 0,
						SoTheDoDoiNha = reader["SoTheDoDoiNha"] != DBNull.Value ? (int)reader["SoTheDoDoiNha"] : 0,
						SoTheDoDoiKhach = reader["SoTheDoDoiKhach"] != DBNull.Value ? (int)reader["SoTheDoDoiKhach"] : 0,
						SoPhatGocDoiNha = reader["SoPhatGocDoiNha"] != DBNull.Value ? (int)reader["SoPhatGocDoiNha"] : 0,
						SoPhatGocDoiKhach = reader["SoPhatGocDoiKhach"] != DBNull.Value ? (int)reader["SoPhatGocDoiKhach"] : 0,


					};

                    matchDetail.MaTdNavigation = GetMatchById(matchId);
                    string leagueId = reader["leagueId"].ToString();

                    leagueName = GetLeagueName(leagueId);

                }


				con.Close();
			}

            ViewBag.leagueName = leagueName;
			return View(matchDetail);
		}

        public IActionResult UpdateMatchDetails(string matchId)
        {
            MatchDetail matchDetail = null;
            string leagueName = null;

            using (SqlConnection con = new SqlConnection(connectString))
            {
                string query = "SELECT * FROM MatchDetails WHERE MaTD =  @MatchId";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.Parameters.AddWithValue("@MatchId", matchId);
                con.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    matchDetail = new MatchDetail
                    {
                        MaTd = reader["MaTD"].ToString(),
						KiemSoatBongDoiNha = reader["KiemSoatBongDoiNha"] != DBNull.Value ? (int)reader["KiemSoatBongDoiNha"] : 50,
						KiemSoatBongDoiKhach = reader["KiemSoatBongDoiKhach"] != DBNull.Value ? (int)reader["KiemSoatBongDoiKhach"] : 50,
						SoCuSutDoiNha = reader["SoCuSutDoiNha"] != DBNull.Value ? (int)reader["SoCuSutDoiNha"] : 0,
						SoCuSutDoiKhach = reader["SoCuSutDoiKhach"] != DBNull.Value ? (int)reader["SoCuSutDoiKhach"] : 0,
						SoDuongChuyenDoiNha = reader["SoDuongChuyenDoiNha"] != DBNull.Value ? (int)reader["SoDuongChuyenDoiNha"] : 0,
						SoDuongChuyenDoiKhach = reader["SoDuongChuyenDoiKhach"] != DBNull.Value ? (int)reader["SoDuongChuyenDoiKhach"] : 0,
						SoLanPhamLoiDoiNha = reader["SoLanPhamLoiDoiNha"] != DBNull.Value ? (int)reader["SoLanPhamLoiDoiNha"] : 0,
						SoLanPhamLoiDoiKhach = reader["SoLanPhamLoiDoiKhach"] != DBNull.Value ? (int)reader["SoLanPhamLoiDoiKhach"] : 0,
						SoTheVangDoiNha = reader["SoTheVangDoiNha"] != DBNull.Value ? (int)reader["SoTheVangDoiNha"] : 0,
						SoTheVangDoiKhach = reader["SoTheVangDoiKhach"] != DBNull.Value ? (int)reader["SoTheVangDoiKhach"] : 0,
						SoTheDoDoiNha = reader["SoTheDoDoiNha"] != DBNull.Value ? (int)reader["SoTheDoDoiNha"] : 0,
						SoTheDoDoiKhach = reader["SoTheDoDoiKhach"] != DBNull.Value ? (int)reader["SoTheDoDoiKhach"] : 0,
						SoPhatGocDoiNha = reader["SoPhatGocDoiNha"] != DBNull.Value ? (int)reader["SoPhatGocDoiNha"] : 0,
						SoPhatGocDoiKhach = reader["SoPhatGocDoiKhach"] != DBNull.Value ? (int)reader["SoPhatGocDoiKhach"] : 0,

					};

                    matchDetail.MaTdNavigation = GetMatchById(matchId);

                }


                con.Close();
            }

            return View(matchDetail);

        }

        [HttpPost]
        public IActionResult UpdateMatchDetails(MatchDetail matchDetail)
        {
            using (SqlConnection con = new SqlConnection(connectString))
            {
                string query = "UPDATE MatchDetails " +
                                "SET KiemSoatBongDoiNha = @KSBDN, KiemSoatBongDoiKhach = @KSBDK, SoCuSutDoiNha = @SCSDN, SoCuSutDoiKhach = @SCSDK, SoDuongChuyenDoiNha = @SDCDN, " +
                                "SoDuongChuyenDoiKhach = @SDCDK, SoLanPhamLoiDoiNha = @SLPLDN, SoLanPhamLoiDoiKhach = @SLPLDK, SoTheVangDoiNha = @STVDN, " + 
                                "SoTheVangDoiKhach = @STVDK, SoTheDoDoiNha = @STDDN, SoTheDoDoiKhach = @STDDK, SoPhatGocDoiNha = @SPGDN, SoPhatGocDoiKhach = @SPGDK " +
                                "WHERE MaTD = @MatchId;" +
                                "UPDATE Matchs " + 
                                "SET SoBanThangDoiNha = @SBTDN, SoBanThangDoiKhach = @SBTDK, MatchDate = @MD WHERE MaTD = @MatchId";


                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.Parameters.AddWithValue("@MatchId", matchDetail.MaTd);
                sqlCommand.Parameters.AddWithValue("@KSBDN", matchDetail.KiemSoatBongDoiNha);
                sqlCommand.Parameters.AddWithValue("@KSBDK", matchDetail.KiemSoatBongDoiKhach);
                sqlCommand.Parameters.AddWithValue("@SCSDN", matchDetail.SoCuSutDoiNha);
                sqlCommand.Parameters.AddWithValue("@SCSDK", matchDetail.SoCuSutDoiKhach);
                sqlCommand.Parameters.AddWithValue("@SDCDN", matchDetail.SoDuongChuyenDoiNha);
                sqlCommand.Parameters.AddWithValue("@SDCDK", matchDetail.SoDuongChuyenDoiKhach);
                sqlCommand.Parameters.AddWithValue("@SLPLDN", matchDetail.SoLanPhamLoiDoiNha);
                sqlCommand.Parameters.AddWithValue("@SLPLDK", matchDetail.SoLanPhamLoiDoiKhach);
                sqlCommand.Parameters.AddWithValue("@STVDN", matchDetail.SoTheVangDoiNha);
                sqlCommand.Parameters.AddWithValue("@STVDK", matchDetail.SoTheVangDoiKhach);
                sqlCommand.Parameters.AddWithValue("@STDDN", matchDetail.SoTheDoDoiNha);
                sqlCommand.Parameters.AddWithValue("@STDDK", matchDetail.SoTheDoDoiKhach);
                sqlCommand.Parameters.AddWithValue("@SPGDN", matchDetail.SoPhatGocDoiNha);
                sqlCommand.Parameters.AddWithValue("@SPGDK", matchDetail.SoPhatGocDoiKhach);
                sqlCommand.Parameters.AddWithValue("@SBTDN", matchDetail.MaTdNavigation.SoBanThangDoiNha);
                sqlCommand.Parameters.AddWithValue("@SBTDK", matchDetail.MaTdNavigation.SoBanThangDoiKhach);
                sqlCommand.Parameters.AddWithValue("@MD", matchDetail.MaTdNavigation.MatchDate);

                con.Open();
                sqlCommand.ExecuteNonQuery();



                con.Close();

            }


            return RedirectToAction("MatchDetails", new {matchId = matchDetail.MaTd });

        }

        public Match GetMatchById(string matchId)
        {
            Match match = null;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                string query = "SELECT * FROM Matchs WHERE MaTD = @MatchId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MatchId", matchId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    match = new Match
                    {
                        MaTd = reader["MaTD"].ToString(),
                        MaDoiNha = reader["MaDoiNha"].ToString(),
                        MaDoiKhach = reader["MaDoiKhach"].ToString(),
                        SoBanThangDoiNha = (int)reader["SoBanThangDoiNha"],
                        SoBanThangDoiKhach = (int)reader["SoBanThangDoiKhach"],
                        MatchDate = DateTime.Parse(reader["MatchDate"].ToString()),                       
                    };

                    match.MaDoiNhaNavigation = GetClubById(match.MaDoiNha);
                    match.MaDoiKhachNavigation = GetClubById(match.MaDoiKhach);
                }

                connection.Close();
            }

            return match;
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

            // Shuffle the list of clubs
            Random random = new Random();
            int n = clubs.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                ClubInLeague temp = clubs[k];
                clubs[k] = clubs[n];
                clubs[n] = temp;
            }

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


        public List<League> GetLeaguesList(string userId)
        {
            List<League> list = new();
            string query = "";

			if (GetLoaiUsersFromMaUsers(userId) == 0)
            {
				query = "SELECT * FROM League";

			}
            else
            {
				query = "SELECT * FROM League where UserId = @userId";
			}
            SqlConnection con = new SqlConnection(connectString);
            con.Open();
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

        private string GetLeagueName(string leagueId)
        {
            string leagueName = string.Empty;
            string query = "SELECT LeagueName FROM League WHERE MaLeague = @LeagueId";

            using (SqlConnection con = new SqlConnection(connectString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.Parameters.AddWithValue("@LeagueId", leagueId);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    leagueName = reader["LeagueName"].ToString();
                }

                con.Close();
            }

            return leagueName;
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
