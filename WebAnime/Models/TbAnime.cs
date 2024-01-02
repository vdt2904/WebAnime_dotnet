using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbAnime
{
    public string MaAnime { get; set; } = null!;

    public string? Anime { get; set; }

    public string? Anh { get; set; }

    public DateTime? NgayPhatSong { get; set; }

    public string? ThongTin { get; set; }

    public string? MaHp { get; set; }

    public int? TongSoTap { get; set; }

    public string? MaLp { get; set; }

    public bool? Lp { get; set; }

    public virtual TbHangPhim? MaHpNavigation { get; set; }

    public virtual TbLoaiPhim? MaLpNavigation { get; set; }

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();

    public virtual ICollection<TbReview> TbReviews { get; set; } = new List<TbReview>();

    public virtual ICollection<TbTapPhim> TbTapPhims { get; set; } = new List<TbTapPhim>();

    public virtual ICollection<TbTlanime> TbTlanimes { get; set; } = new List<TbTlanime>();

    public virtual ICollection<TbWorth> TbWorths { get; set; } = new List<TbWorth>();
}
