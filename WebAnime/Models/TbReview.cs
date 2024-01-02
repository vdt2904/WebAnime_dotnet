using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbReview
{
    public int Id { get; set; }

    public string? MaAnime { get; set; }

    public string? MaNd { get; set; }

    public string? Review { get; set; }

    public int? Rate { get; set; }

    public DateTime? NgayReview { get; set; }

    public virtual TbAnime? MaAnimeNavigation { get; set; }

    public virtual TbNguoiDung? MaNdNavigation { get; set; }
}
