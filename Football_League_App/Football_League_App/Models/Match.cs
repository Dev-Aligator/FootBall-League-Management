using System;
using System.Collections.Generic;

namespace Football_League_App.Models;

public partial class Match
{
    public int Id { get; set; }

    public string MaTd { get; set; } = null!;

    public string MaDoiNha { get; set; } = null!;
    public Club HomeClub { get; set; }


    public string MaDoiKhach { get; set; } = null!;
    public Club AwayClub { get; set; }

    public int? SoBanThangDoiNha { get; set; }

    public int? SoBanThangDoiKhach { get; set; }

    public DateTime MatchDate { get; set; }


    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual Club MaDoiKhachNavigation { get; set; } = null!;

    public virtual Club MaDoiNhaNavigation { get; set; } = null!;
}
