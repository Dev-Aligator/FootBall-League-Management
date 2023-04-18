namespace Football_League_App.Models
{
    public class Matchs
    {
        public string MatchID { get; set; } = null!;

        public virtual Clubs HomeClubID { get; set; } = null!;

        public virtual Clubs AwayClubID { get; set; } = null!;

        public int NumGoalHomeClub { get; set; }

        public int NumGoalAwayClub { get; set; }

        public int NumYellowCardHomeClub { get; set; }

        public int NumYellowCardAwayClub { get; set; }

        public int NumRedCardHomeClub { get; set; }

        public int NumRedCardAwayClub { get; set; }
    }
}
