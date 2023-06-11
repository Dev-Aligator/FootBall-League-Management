using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Football_League_App.Models;

public partial class Player
{
    public int Id { get; set; }

    [Key]
    [DisplayName("Mã Cầu Thủ")]
    public string MaCt { get; set; } = null!;

    [Required]
    [DisplayName("Tên Cầu Thủ")]
    public string TenCt { get; set; } = null!;

    [Required]
    [DisplayName("Tên Cầu Thủ")]
    public int? LoaiCt { get; set; }

    [DisplayName("Quốc Tịch")]
    public string QuocTich { get; set; }

    [DisplayName("Lương")]
    public int Luong { get; set; }

    [DisplayName("Chiều Cao")]
    public int ChieuCao { get; set; }

    [DisplayName("Cân Nặng")]
    public int CanNang { get; set; }

    [DisplayName("Vị Trí Chính")]
    public string ViTriChinh { get; set; }

    [DisplayName("Vị Trí Phụ")]
    public string ViTriPhu { get; set; }

    [DisplayName("Ngày Sinh")]
    public DateTime NgaySinh { get; set; }

    [DisplayName("Số Áo")]
    public int SoAo { get; set; }

    [DisplayName("Chân Thuận")]
    public string ChanThuan { get; set; }

    [DisplayName("Mã CLB")]
    public string MaClb { get; set; } = null!;

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual Club MaClbNavigation { get; set; } = null!;
}
