using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbVip
{
    public string LoaiVip { get; set; } = null!;

    public int? ThoiGianSd { get; set; }

    public double? Gia { get; set; }

    public virtual ICollection<TbHoaDon> TbHoaDons { get; set; } = new List<TbHoaDon>();
}
