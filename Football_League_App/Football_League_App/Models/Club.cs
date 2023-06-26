using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Football_League_App.Models;

public partial class Club
{
    public int Id { get; set; }

    public string MaClb { get; set; } = null!;

    public string TenClb { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string TenSvd { get; set; } = null!;

    public string MaCt { get; set; } = null!;

    [Required]
	 //public IFormFile ClubPhoto { get; set; } 

    public string? ImgPath { get; set; } 

    public virtual Player MaCtNavigation { get; set; } = null!;

    public virtual ICollection<Match> MatchMaDoiKhachNavigations { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchMaDoiNhaNavigations { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

	public string UserId { get; set; }
    public User User { get; set; } = null!;
}
