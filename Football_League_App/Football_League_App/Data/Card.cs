using System;
using System.Collections.Generic;

namespace Football_League_App.Data
{
    public class Card
    {
        public int Id { get; set; }

        public string MaThe { get; set; } = null!;

        public string? LoaiThe { get; set; }

        public int? Phut { get; set; } = null!;

        public int? PhutBuGio { get; set; }

        public string MaCtNhanThe { get; set; } = null!;

        public string MaTd { get; set; } = null!;

        public virtual Player MaCtNhanTheNavigation { get; set; } = null!;

        public virtual Match MaTdNavigation { get; set; } = null!;
    }
}
