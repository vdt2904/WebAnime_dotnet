namespace WebAnime.Models.ViewModel
{
    public class OurBlogViewModel
    {
        public int? IdBlog { get; set; }
        public string? TenBlog { get; set; }
        public string? AnhBlog { get; set;}
        public string? ThongTinBlog { get; set; }

        public DateTime? ngaydang { get; set; }
        public List<AnimeBlog> animeBlogs { get; set; }
        public List<TheLoaiBlog> theLoaiBlogs { get; set; }
    }
}
