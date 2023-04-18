namespace Football_League_App.Models
{
    public class MatchDetails
    {
        public string MatchDetailID { get; set; } = null!;

        public virtual Matchs MatchID { get; set; } = null!;

        public int NumofPosseisionHomeClub { get; set; }

        public int NumofPosseisionAwayClub { get; set; }

        public int NumofShootHomeClub { get; set; }

        public int NumofShootAwayClub { get; set; }

        public int NumofPassHomeClub { get; set; }

        public int NumofPassAwayClub { get; set; }

        public int NumofFaultHomeClub { get; set; }

        public int NumofFaultAwayClub { get; set; }

        public virtual Players YellowCardPlayerID { get; set; } = null!;

        public virtual Players RedCardPlayerID { get; set; } = null!;
    }
}
