using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbResetPass
{
    public int Id { get; set; }

    public string? MaNd { get; set; }

    public string? Email { get; set; }

    public string? ResetPass { get; set; }

    public DateTime? ThoiGian { get; set; }

    public string? Token { get; set; }

    public virtual TbNguoiDung? MaNdNavigation { get; set; }
}
