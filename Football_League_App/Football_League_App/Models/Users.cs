namespace Football_League_App.Models
{
    public class Users
    { 
        public int UserID { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? PhoneNumber { get; set; } // string? != string: Kiểu string? được phép Null

        public string Email { get; set; } = null!;

        public int UserType { get; set; }

    }
}
