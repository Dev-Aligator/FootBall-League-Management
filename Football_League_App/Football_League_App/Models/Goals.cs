namespace Football_League_App.Models
{
    public class Goals
    {
        public string GoalId { get; set; } = null!;

        public string? GoalType { get; set; }

        public DateTime? GoalTime { get; set; }

        public virtual Players PlayerScoreID { get; set; } = null!;

        public virtual Players PlayerAssitID { get; set; } = null!;
        
        public virtual Matchs MatchID { get; set; } = null!;

    }
}
