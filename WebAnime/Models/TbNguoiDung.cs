using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbNguoiDung
{
    public string MaNd { get; set; } = null!;

    public string? TenNguoiDung { get; set; }

    public string? Email { get; set; }

    public string? Sdt { get; set; }

    public string? Pasword { get; set; }

    public int? LoaiNd { get; set; }

    public int? Xu { get; set; }

    public virtual ICollection<TbComment> TbComments { get; set; } = new List<TbComment>();

    public virtual ICollection<TbHoaDon> TbHoaDons { get; set; } = new List<TbHoaDon>();

    public virtual ICollection<TbResetPass> TbResetPasses { get; set; } = new List<TbResetPass>();

    public virtual ICollection<TbReview> TbReviews { get; set; } = new List<TbReview>();

    public virtual ICollection<TbView> TbViews { get; set; } = new List<TbView>();
}
