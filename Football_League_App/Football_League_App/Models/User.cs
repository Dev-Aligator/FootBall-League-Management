using System;
using System.Collections.Generic;

namespace Football_League_App.Models;

public partial class User
{
    public int Id { get; set; }

    public string MaUsers { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Sdt { get; set; }

    public string Email { get; set; } = null!;

    public int LoaiUsers { get; set; }
}
