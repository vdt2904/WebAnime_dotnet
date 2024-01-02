using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbBlog
{
    public int Id { get; set; }

    public string? MaAnime { get; set; }

    public int? Idblog { get; set; }

    public string? Trailer { get; set; }

    public virtual TbOurBlog? IdblogNavigation { get; set; }

    public virtual TbAnime? MaAnimeNavigation { get; set; }
}
