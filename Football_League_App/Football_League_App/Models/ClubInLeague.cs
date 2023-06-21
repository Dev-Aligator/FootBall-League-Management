using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Football_League_App.Models;

public partial class ClubInLeague
{
    [ForeignKey("Club")]
    public String ClubId { get; set; }
    public Club Club { get; set; }

    [ForeignKey("League")]
    public String LeagueId { get; set; }
    public League League { get; set; }

    public int MatchesPlayed { get; set; }
    public int Points { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
}
