namespace Football_League_App.Models
{
    public class Players
    {
        public string PlayerID { get; set; } = null!;

        public string PlayerName { get; set; } = null!;

        public int PlayerType { get; set; }

        public decimal Salary { get; set; }

        public int? Height { get; set; }
        
        public int? Weight { get; set; }

        public string? MainPos { get; set; }

        public string? SubPos { get; set; }

        public DateTime? DayofBirth { get; set; }

        public int? PlayerNumber { get; set; }

        public string? MainLeg { get; set; }

        public virtual Clubs ClubID { get; set; } = null!;

    }
}
