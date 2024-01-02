using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbComment
{
    public int Id { get; set; }

    public string? MaNd { get; set; }

    public string? MaTp { get; set; }

    public string? Comment { get; set; }

    public DateTime? NgayComent { get; set; }

    public virtual TbNguoiDung? MaNdNavigation { get; set; }

    public virtual TbTapPhim? MaTpNavigation { get; set; }
}
