using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbView
{
    public int Id { get; set; }

    public string? MaNd { get; set; }

    public string? MaTp { get; set; }

    public int? Slviews { get; set; }

    public DateTime? NgayXem { get; set; }

    public virtual TbNguoiDung? MaNdNavigation { get; set; }

    public virtual TbTapPhim? MaTpNavigation { get; set; }
}
