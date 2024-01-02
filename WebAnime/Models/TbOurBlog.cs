using System;
using System.Collections.Generic;

namespace WebAnime.Models;

public partial class TbOurBlog
{
    public int Idblog { get; set; }

    public string? TenBlog { get; set; }

    public string? Anh { get; set; }

    public string? ThongTin { get; set; }

    public DateTime? NgayDang { get; set; }

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();
}
