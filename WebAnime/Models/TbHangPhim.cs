using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbHangPhim
{
    public string MaHp { get; set; } = null!;

    public string? TenHangPhim { get; set; }

    public virtual ICollection<TbAnime> TbAnimes { get; set; } = new List<TbAnime>();
}
