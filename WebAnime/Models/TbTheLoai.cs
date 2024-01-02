using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbTheLoai
{
    public string MaTl { get; set; } = null!;

    public string? TheLoai { get; set; }

    public string? ThongTin { get; set; }

    public virtual ICollection<TbTlanime> TbTlanimes { get; set; } = new List<TbTlanime>();
}
