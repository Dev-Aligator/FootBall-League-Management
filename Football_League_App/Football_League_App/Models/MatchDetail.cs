using System;
using System.Collections.Generic;

namespace Football_League_App.Models;

public partial class MatchDetail
{
    public int Id { get; set; }

    public string? MaCttd { get; set; }

    public string MaTd { get; set; } = null!;

    public int? KiemSoatBongDoiNha { get; set; }

    public int? KiemSoatBongDoiKhach { get; set; }

    public int? SoCuSutDoiNha { get; set; }

    public int? SoCuSutDoiKhach { get; set; }

    public int? SoDuongChuyenDoiNha { get; set; }

    public int? SoDuongChuyenDoiKhach { get; set; }

    public int? SoLanPhamLoiDoiNha { get; set; }

    public int? SoLanPhamLoiDoiKhach { get; set; }

    public int? SoTheVangDoiNha { get; set; }

    public int? SoTheVangDoiKhach { get; set; }

    public int? SoTheDoDoiNha { get; set; }

    public int? SoTheDoDoiKhach { get; set; }

    public int? SoPhatGocDoiNha { get; set; }

    public int? SoPhatGocDoiKhach { get; set; }

    public virtual Match MaTdNavigation { get; set; } = null!;
}
