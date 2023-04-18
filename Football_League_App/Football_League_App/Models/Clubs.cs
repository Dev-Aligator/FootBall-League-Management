namespace Football_League_App.Models
{
    public class Clubs
    {
        public string ClubId { get; set; } = null!; 

        public string ClubName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string HomeName { get; set; } = null!;

        public virtual Players PlayerID { get; set; } = null!;
    }
}
