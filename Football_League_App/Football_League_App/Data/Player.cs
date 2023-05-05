using System;
using System.Collections.Generic;

namespace Football_League_App.Data;

public partial class Player
{
    public int Id { get; set; }

    public string MaCt { get; set; } = null!;

    public string TenCt { get; set; } = null!;

    public int LoaiCt { get; set; }

    public decimal? Luong { get; set; }

    public int? ChieuCao { get; set; }

    public int? CanNang { get; set; }

    public string? ViTriChinh { get; set; }

    public string? ViTriPhu { get; set; }

    public DateTime? NgaySinh { get; set; }

    public int? SoAo { get; set; }

    public string? ChanThuan { get; set; }

    public string MaClb { get; set; } = null!;

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual ICollection<Goal> GoalMaCtghiBanNavigations { get; set; } = new List<Goal>();

    public virtual ICollection<Goal> GoalMaCtkienTaoNavigations { get; set; } = new List<Goal>();

    public virtual Club MaClbNavigation { get; set; } = null!;
}
