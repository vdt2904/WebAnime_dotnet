using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Xml.Linq;
using WebAnime.Models;
using WebAnime.Models.Authentication;
using WebAnime.Models.ViewModel;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAnime.Controllers
{

    public class HomeController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            AnimeViewModel animeView = new AnimeViewModel();
            #region trend
            //  trend
            animeView.trendingList = new List<TbAnimeViewModel>();
            var maTlMax = db.TbTlanimes
                .GroupBy(tl => tl.MaTl)
                .Select(g => new { MaTl = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault()?.MaTl;
            var trend = db.TbAnimes
                .Join(db.TbTlanimes, a => a.MaAnime, tl => tl.MaAnime, (a, tl) => new { a, tl })
                .Join(db.TbTheLoais, al => al.tl.MaTl, t => t.MaTl, (al, t) => new { al, t })
                .Join(db.TbTapPhims, alt => alt.al.a.MaAnime, tp => tp.MaAnime, (alt, tp) => new { alt, tp })
                .Where(x => x.alt.t.MaTl == maTlMax)
                .GroupBy(x => x.alt.al.a.MaAnime)
                .Select(x => new TbAnimeViewModel
                {
                    MaAnime = x.Key,
                    Anime = x.First().alt.al.a.Anime,
                    Anh = x.First().alt.al.a.Anh,
                    TongSoTap = x.First().alt.al.a.TongSoTap,
                    TapHienTai = x.Max(y => y.tp.Tap),
                    TongView = x.Sum(y => y.tp.Views),
                    TongComment = x.Sum(y => y.tp.Comments)
                })
                .OrderByDescending(g => g.TongView)
                .Take(6)
                .ToList();
            animeView.trendingList.AddRange(trend);
            #endregion
            #region pupular
            // popular
            animeView.popularList = new List<TbAnimeViewModel>();
            var result = (
                from anime in db.TbAnimes
                join tapPhim in db.TbTapPhims on anime.MaAnime equals tapPhim.MaAnime
                join tlAnime in db.TbTlanimes on anime.MaAnime equals tlAnime.MaAnime
                join theLoai in db.TbTheLoais on tlAnime.MaTl equals theLoai.MaTl
                group tapPhim by new { anime.MaAnime, anime.Anime, anime.Anh, anime.TongSoTap } into g
                orderby g.Sum(x => x.Views) descending
                select new TbAnimeViewModel
                {
                    MaAnime = g.Key.MaAnime,
                    Anime = g.Key.Anime,
                    Anh = g.Key.Anh,
                    TongSoTap = g.Key.TongSoTap,
                    TapHienTai = db.TbTapPhims.Where(x => x.MaAnime == g.Key.MaAnime).Max(x => x.Tap),
                    TongView = g.Sum(x => x.Views),
                    TongComment = g.Sum(x => x.Comments)
                }
                ).Take(6).ToList();
            animeView.popularList = result;
            #endregion
            #region rêcntly
            // recently
            animeView.recentlyList = new List<TbAnimeViewModel>();
            var recently = db.TbAnimes
                .Join(db.TbTapPhims, anime => anime.MaAnime, tapPhim => tapPhim.MaAnime, (anime, tapPhim) => new { anime, tapPhim })
                .GroupBy(x => new { x.anime.MaAnime, x.anime.Anime, x.anime.Anh, x.anime.TongSoTap, x.tapPhim.NgayPhatSong, x.tapPhim.MaTp })
                .Where(g => g.Key.NgayPhatSong == db.TbTapPhims.Where(tp => tp.MaAnime == g.Key.MaAnime).Max(tp => tp.NgayPhatSong))
                .OrderByDescending(x => x.Key.NgayPhatSong)
                .Take(6)
                .Select(g => new TbAnimeViewModel
                {
                    MaAnime = g.Key.MaAnime,
                    Anime = g.Key.Anime,
                    Anh = g.Key.Anh,
                    TongSoTap = g.Key.TongSoTap,
                    TapHienTai = g.Max(x => x.tapPhim.Tap),
                    TongComment = g.Sum(x => x.tapPhim.Comments),
                    TongView = g.Sum(x => x.tapPhim.Views)
                })
                .ToList();

            animeView.recentlyList = recently;
            #endregion
            #region Live
            //Live action
            animeView.liveList = new List<TbAnimeViewModel>();
            var liveActionList = db.TbAnimes
                .Join(db.TbTapPhims, anime => anime.MaAnime, tapPhim => tapPhim.MaAnime, (anime, tapPhim) => new { anime, tapPhim })
                .Join(db.TbLoaiPhims, animeTapPhim => animeTapPhim.anime.MaLp, loaiPhim => loaiPhim.MaLp, (animeTapPhim, loaiPhim) => new { animeTapPhim.anime, animeTapPhim.tapPhim, loaiPhim })
                .Where(x => x.loaiPhim.LoaiPhim == "Live Action")
                .GroupBy(x => new { x.anime.MaAnime, x.anime.Anime, x.anime.Anh, x.anime.TongSoTap, x.tapPhim.NgayPhatSong, x.tapPhim.MaTp })
                .Select(g => new TbAnimeViewModel
                {
                    MaAnime = g.Key.MaAnime,
                    Anime = g.Key.Anime,
                    Anh = g.Key.Anh,
                    TongSoTap = g.Key.TongSoTap,
                    TapHienTai = g.Max(x => x.tapPhim.Tap),
                    TongComment = g.Sum(x => x.tapPhim.Comments),
                    TongView = g.Sum(x => x.tapPhim.Views)
                })
                .OrderByDescending(x => x.MaAnime)
                .Take(6)
                .ToList();

            animeView.liveList = liveActionList;
            #endregion
            return View(animeView);
        }

        public IActionResult Trending(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var maTlMax = db.TbTlanimes
                .GroupBy(tl => tl.MaTl)
                .Select(g => new { MaTl = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault()?.MaTl;
            var trend = db.TbAnimes
                .Join(db.TbTlanimes, a => a.MaAnime, tl => tl.MaAnime, (a, tl) => new { a, tl })
                .Join(db.TbTheLoais, al => al.tl.MaTl, t => t.MaTl, (al, t) => new { al, t })
                .Join(db.TbTapPhims, alt => alt.al.a.MaAnime, tp => tp.MaAnime, (alt, tp) => new { alt, tp })
                .Where(x => x.alt.t.MaTl == maTlMax)
                .GroupBy(x => x.alt.al.a.MaAnime)
                .Select(x => new TbAnimeViewModel
                {
                    MaAnime = x.Key,
                    Anime = x.First().alt.al.a.Anime,
                    Anh = x.First().alt.al.a.Anh,
                    TongSoTap = x.First().alt.al.a.TongSoTap,
                    TapHienTai = x.Max(y => y.tp.Tap),
                    TongView = x.Sum(y => y.tp.Views),
                    TongComment = x.Sum(y => y.tp.Comments)
                })
                .OrderByDescending(g => g.TongView)
                .ToList();
            PagedList<TbAnimeViewModel> lst = new PagedList<TbAnimeViewModel>(trend, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult Popular(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var result = (
                from anime in db.TbAnimes
                join tapPhim in db.TbTapPhims on anime.MaAnime equals tapPhim.MaAnime
                join tlAnime in db.TbTlanimes on anime.MaAnime equals tlAnime.MaAnime
                join theLoai in db.TbTheLoais on tlAnime.MaTl equals theLoai.MaTl
                group tapPhim by new { anime.MaAnime, anime.Anime, anime.Anh, anime.TongSoTap } into g
                orderby g.Sum(x => x.Views) descending
                select new TbAnimeViewModel
                {
                    MaAnime = g.Key.MaAnime,
                    Anime = g.Key.Anime,
                    Anh = g.Key.Anh,
                    TongSoTap = g.Key.TongSoTap,
                    TapHienTai = db.TbTapPhims.Where(x => x.MaAnime == g.Key.MaAnime).Max(x => x.Tap),
                    TongView = g.Sum(x => x.Views),
                    TongComment = g.Sum(x => x.Comments)
                }
                ).ToList();
            PagedList<TbAnimeViewModel> lst = new PagedList<TbAnimeViewModel>(result, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult Recently(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var recently = db.TbAnimes
               .Join(db.TbTapPhims, anime => anime.MaAnime, tapPhim => tapPhim.MaAnime, (anime, tapPhim) => new { anime, tapPhim })
               .GroupBy(x => new { x.anime.MaAnime, x.anime.Anime, x.anime.Anh, x.anime.TongSoTap, x.tapPhim.NgayPhatSong, x.tapPhim.MaTp })
               .Where(g => g.Key.NgayPhatSong == db.TbTapPhims.Where(tp => tp.MaAnime == g.Key.MaAnime).Max(tp => tp.NgayPhatSong))
               .OrderByDescending(x => x.Key.NgayPhatSong)
               .Select(g => new TbAnimeViewModel
               {
                   MaAnime = g.Key.MaAnime,
                   Anime = g.Key.Anime,
                   Anh = g.Key.Anh,
                   TongSoTap = g.Key.TongSoTap,
                   TapHienTai = g.Max(x => x.tapPhim.Tap),
                   TongComment = g.Sum(x => x.tapPhim.Comments),
                   TongView = g.Sum(x => x.tapPhim.Views)
               })
               .ToList();
            PagedList<TbAnimeViewModel> lst = new PagedList<TbAnimeViewModel>(recently, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult Live(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var liveActionList = db.TbAnimes
            .Join(db.TbTapPhims, anime => anime.MaAnime, tapPhim => tapPhim.MaAnime, (anime, tapPhim) => new { anime, tapPhim })
            .Join(db.TbLoaiPhims, animeTapPhim => animeTapPhim.anime.MaLp, loaiPhim => loaiPhim.MaLp, (animeTapPhim, loaiPhim) => new { animeTapPhim.anime, animeTapPhim.tapPhim, loaiPhim })
            .Where(x => x.loaiPhim.LoaiPhim == "Live Action")
            .GroupBy(x => new { x.anime.MaAnime, x.anime.Anime, x.anime.Anh, x.anime.TongSoTap, x.tapPhim.NgayPhatSong, x.tapPhim.MaTp })
            .Select(g => new TbAnimeViewModel
            {
                MaAnime = g.Key.MaAnime,
                Anime = g.Key.Anime,
                Anh = g.Key.Anh,
                TongSoTap = g.Key.TongSoTap,
                TapHienTai = g.Max(x => x.tapPhim.Tap),
                TongComment = g.Sum(x => x.tapPhim.Comments),
                TongView = g.Sum(x => x.tapPhim.Views)
            })
            .OrderByDescending(x => x.MaAnime)
            .ToList();
            PagedList<TbAnimeViewModel> lst = new PagedList<TbAnimeViewModel>(liveActionList, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult AnimeToTheLoai(int? page, string theloai)
        {
            ViewBag.theloai = theloai;
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var result = db.TbAnimes
            .Join(db.TbTapPhims, anime => anime.MaAnime, tapPhim => tapPhim.MaAnime, (anime, tapPhim) => new { anime, tapPhim })
            .Join(db.TbTlanimes, a => a.anime.MaAnime, tl => tl.MaAnime, (a, tl) => new { a, tl })
            .Join(db.TbTheLoais, al => al.tl.MaTl, theLoai => theLoai.MaTl, (al, theLoai) => new { al, theLoai })
            .Where(joined => joined.theLoai.TheLoai == theloai)
            .GroupBy(joined => new
            {
                joined.al.a.anime.MaAnime,
                joined.al.a.anime.Anime,
                joined.al.a.anime.Anh,
                joined.al.a.anime.TongSoTap
            })
            .Select(g => new TbAnimeViewModel
            {
                MaAnime = g.Key.MaAnime,
                Anime = g.Key.Anime,
                Anh = g.Key.Anh,
                TongSoTap = g.Key.TongSoTap,
                TapHienTai = g.OrderByDescending(x => x.al.a.tapPhim.NgayPhatSong)
                              .ThenByDescending(x => x.al.a.tapPhim.Tap)
                              .Select(x => x.al.a.tapPhim.Tap)
                              .FirstOrDefault(),
                TongComment = g.Sum(x => x.al.a.tapPhim.Comments),
                TongView = g.Sum(x => x.al.a.tapPhim.Views)
            })
            .OrderByDescending(x => x.TongView)
            .ToList();

            PagedList<TbAnimeViewModel> lst = new PagedList<TbAnimeViewModel>(result, pageNumber, pageSize);
            return View(lst);
        }
        [Authentication1]
        public IActionResult ChiTietAnime(string ma)
        {
            TbAnimeViewModel a = new TbAnimeViewModel();
            var result = db.TbAnimes
            .Join(db.TbTapPhims, anime => anime.MaAnime, tapPhim => tapPhim.MaAnime, (anime, tapPhim) => new { anime, tapPhim })
            .Join(db.TbTlanimes, a => a.anime.MaAnime, tl => tl.MaAnime, (a, tl) => new { a, tl })
            .Join(db.TbTheLoais, al => al.tl.MaTl, theLoai => theLoai.MaTl, (al, theLoai) => new { al, theLoai })
            .Where(joined => joined.al.a.anime.MaAnime == ma)
            .GroupBy(joined => new
            {
                joined.al.a.anime.MaAnime,
                joined.al.a.anime.Anime,
                joined.al.a.anime.Anh,
                joined.al.a.anime.TongSoTap
            }).Select(g => new TbAnimeViewModel
            {
                MaAnime = g.Key.MaAnime,
                Anime = g.Key.Anime,
                Anh = g.Key.Anh,
                TongSoTap = g.Key.TongSoTap,
                TapHienTai = g.OrderByDescending(x => x.al.a.tapPhim.NgayPhatSong)
                              .ThenByDescending(x => x.al.a.tapPhim.Tap)
                              .Select(x => x.al.a.tapPhim.Tap)
                              .FirstOrDefault(),
                TongComment = g.Sum(x => x.al.a.tapPhim.Comments),
                TongView = g.Sum(x => x.al.a.tapPhim.Views)
            }).FirstOrDefault();

            return View(result);
        }
        [Authentication1]
        public IActionResult WatchAnime(string ma , int t)
        {
            var b = db.TbNguoiDungs.FirstOrDefault(x => x.MaNd == HttpContext.Session.GetString("DangNhap"));
            var a = db.TbAnimes.FirstOrDefault(x => x.MaAnime == ma);
            if(b.LoaiNd == 0 && a.Lp == true)
            {
                return RedirectToAction("ThanhToanp", "ThanhToan");
            }
            var ani = db.TbTapPhims.FirstOrDefault(x => x.MaAnime == ma && x.Tap == t);
            return View(ani);
        }

        public IActionResult OurBlog()
        {
            
            var latestBlog = db.TbOurBlogs.OrderByDescending(b => b.Idblog).FirstOrDefault();
            var query = from anime in db.TbAnimes
                        join blog in db.TbBlogs on anime.MaAnime equals blog.MaAnime
                        where blog.Idblog == latestBlog.Idblog
                        select new AnimeBlog { MaAnine = anime.MaAnime, Anime = anime.Anime, Anh = anime.Anh, Trailer = blog.Trailer ,ThongTin = anime.ThongTin};
            List<AnimeBlog> animes = query.ToList();
            List<string> ma = animes.Select(x => x.MaAnine).ToList();
            var query1 = from anime in db.TbAnimes
                        join tlAnime in db.TbTlanimes on anime.MaAnime equals tlAnime.MaAnime
                        join theLoai in db.TbTheLoais on tlAnime.MaTl equals theLoai.MaTl
                        where ma.Contains(anime.MaAnime)
                         select new TheLoaiBlog { theloai = theLoai.TheLoai };
            var distinctGenres = query1.Distinct().ToList();
            List<TheLoaiBlog> theLoais = distinctGenres;
            OurBlogViewModel ourBlog = new OurBlogViewModel {
                IdBlog = latestBlog.Idblog,
                TenBlog = latestBlog.TenBlog,
                AnhBlog = latestBlog.Anh,
                ThongTinBlog = latestBlog.ThongTin,
                ngaydang = latestBlog.NgayDang,
                animeBlogs = animes,
                theLoaiBlogs = theLoais
            };
            return View(ourBlog);
        }

        public IActionResult TimKiem(int? page, string search)
        {
            ViewBag.Search = search;
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            string[] keywords = search.Split(' ');

            var result = db.TbAnimes
                .AsEnumerable()
                .Select(anime => new
                {
                    Anime = anime,
                    MatchedKeywordCount = keywords.Count(keyword =>
                        anime.Anime.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0)
                })
                .Where(x => x.MatchedKeywordCount > 0)
                .OrderByDescending(x => x.MatchedKeywordCount)
                .Select(x => new TbAnime
                {
                    MaAnime = x.Anime.MaAnime,
                    Anime = x.Anime.Anime,
                    Anh = x.Anime.Anh,
                    TongSoTap = x.Anime.TongSoTap
                })
                .ToList();

            PagedList<TbAnime> lst = new PagedList<TbAnime>(result, pageNumber, pageSize);
            return View(lst);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}