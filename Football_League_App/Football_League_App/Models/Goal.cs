using System;
using System.Collections.Generic;

namespace Football_League_App.Models;

public partial class Goal
{
    public int Id { get; set; }

    public string MaBt { get; set; } = null!;

    public string? LoaiBt { get; set; }

    public int Phut { get; set; }

    public int? PhutBuGio { get; set; }

    public string MaCtghiBan { get; set; } = null!;

    public string MaCtkienTao { get; set; } = null!;

    public string MaTd { get; set; } = null!;

    public virtual Match MaTdNavigation { get; set; } = null!;
}
