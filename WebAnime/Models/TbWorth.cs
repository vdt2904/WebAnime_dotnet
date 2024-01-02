using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbWorth
{
    public int Id { get; set; }

    public string? MaAnime { get; set; }

    public virtual TbAnime? MaAnimeNavigation { get; set; }
}
