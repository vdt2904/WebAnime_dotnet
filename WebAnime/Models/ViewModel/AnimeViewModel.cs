namespace WebAnime.Models.ViewModel
{
    public class AnimeViewModel
    {
        public List<TbAnimeViewModel> trendingList { get; set; }
        public List<TbAnimeViewModel> popularList { get; set; }
        public List<TbAnimeViewModel> recentlyList { get; set; }
        public List<TbAnimeViewModel> liveList { get; set; }

    }
}
