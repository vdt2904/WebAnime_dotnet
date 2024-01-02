using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbHoaDon
{
    public string SoHd { get; set; } = null!;

    public string? MaNd { get; set; }

    public string? LoaiVip { get; set; }

    public DateTime? NgayMua { get; set; }

    public DateTime? NgayHetHan { get; set; }

    public virtual TbVip? LoaiVipNavigation { get; set; }

    public virtual TbNguoiDung? MaNdNavigation { get; set; }
}
