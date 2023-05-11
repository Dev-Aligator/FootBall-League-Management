using System;
using System.Collections.Generic;

namespace Football_League_App.Models;

public partial class Card
{
    public int Id { get; set; }

    public string MaThe { get; set; } = null!;

    public int? LoaiThe { get; set; }

    public int Phut { get; set; }

    public int? PhutBuGio { get; set; }

    public string MaCtnhanThe { get; set; } = null!;

    public string MaTd { get; set; } = null!;

    public virtual Player MaCtnhanTheNavigation { get; set; } = null!;
}
