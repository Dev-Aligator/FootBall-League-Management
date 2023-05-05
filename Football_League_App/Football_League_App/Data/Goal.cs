using System;
using System.Collections.Generic;

namespace Football_League_App.Data;

public partial class Goal
{
    public int Id { get; set; }

    public string MaBt { get; set; } = null!;

    public string? LoaiBt { get; set; }

    public DateTime? ThoiDiemGhiBan { get; set; }

    public string MaCtghiBan { get; set; } = null!;

    public string MaCtkienTao { get; set; } = null!;

    public string MaTd { get; set; } = null!;

    public virtual Player MaCtghiBanNavigation { get; set; } = null!;

    public virtual Player MaCtkienTaoNavigation { get; set; } = null!;

    public virtual Match MaTdNavigation { get; set; } = null!;
}
