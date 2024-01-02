using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbTapPhim
{
    public string MaTp { get; set; } = null!;

    public string? MaAnime { get; set; }

    public int? Tap { get; set; }

    public int? Views { get; set; }

    public int? Comments { get; set; }

    public DateTime? NgayPhatSong { get; set; }

    public string? Video { get; set; }

    public string? AnhVideo { get; set; }

    public virtual TbAnime? MaAnimeNavigation { get; set; }

    public virtual ICollection<TbComment> TbComments { get; set; } = new List<TbComment>();

    public virtual ICollection<TbView> TbViews { get; set; } = new List<TbView>();
}
