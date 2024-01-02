using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbLoaiPhim
{
    public string MaLp { get; set; } = null!;

    public string? LoaiPhim { get; set; }

    public virtual ICollection<TbAnime> TbAnimes { get; set; } = new List<TbAnime>();
}
