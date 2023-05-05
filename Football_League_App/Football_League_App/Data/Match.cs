using System;
using System.Collections.Generic;

namespace Football_League_App.Data;

public partial class Match
{
    public int Id { get; set; }

    public string MaTd { get; set; } = null!;

    public string MaDoiNha { get; set; } = null!;

    public string MaDoiKhach { get; set; } = null!;

    public int? SoBanThangDoiNha { get; set; }

    public int? SoBanThangDoiKhach { get; set; }

    public int? SoTheVangDoiNha { get; set; }

    public int? SoTheVangDoiKhach { get; set; }

    public int? SoTheDoDoiNha { get; set; }

    public int? SoTheDoDoiKhach { get; set; }

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual Club MaDoiKhachNavigation { get; set; } = null!;

    public virtual Club MaDoiNhaNavigation { get; set; } = null!;
}
