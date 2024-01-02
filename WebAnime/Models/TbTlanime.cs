using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbTlanime
{
    public string? MaTl { get; set; }

    public string? MaAnime { get; set; }

    public int Id { get; set; }

    public virtual TbAnime? MaAnimeNavigation { get; set; }

    public virtual TbTheLoai? MaTlNavigation { get; set; }
}
