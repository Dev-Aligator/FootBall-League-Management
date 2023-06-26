using System;
using System.Collections.Generic;

namespace Football_League_App.Models;

public partial class League
{
    public int Id { get; set; }

    public string MaLeague { get; set; } = null!;

    public string LeagueName { get; set; } = null!;

    public int MaxTeams { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

	public string UserId { get; set; }
	public User User { get; set; } = null!;

	public bool IsPublic { get; set; }


}
