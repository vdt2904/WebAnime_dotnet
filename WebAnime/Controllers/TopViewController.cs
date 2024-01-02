using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAnime.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopViewController : ControllerBase
    {
        QlAnimeContext db = new QlAnimeContext();
        [HttpGet]
        public IActionResult TopDay()
        {
            var topAnimeToday = (from v in db.TbViews
                                 join tp in db.TbTapPhims on v.MaTp equals tp.MaTp
                                 join a in db.TbAnimes on tp.MaAnime equals a.MaAnime
                                 where v.NgayXem >= DateTime.Today
                                 group v by new { a.MaAnime, a.Anime, a.Anh , a.Lp } into g
                                 orderby g.Count() descending
                                 select new
                                 {
                                     MaAnime = g.Key.MaAnime,
                                     Anime = g.Key.Anime,
                                     Anh = g.Key.Anh,
                                     Lp = g.Key.Lp,
                                     TotalViews = g.Count()
                                 }
                    ).Take(5).ToList();

            var maAnimeList = topAnimeToday.Select(a => a.MaAnime).ToList();

            var maxTapList = (from tp in db.TbTapPhims
                              where maAnimeList.Contains(tp.MaAnime)
                              group tp by tp.MaAnime into g
                              select new { MaAnime = g.Key, MaxTap = g.Max(tp => tp.Tap) }
                            ).ToList();

            var c = (from a in topAnimeToday
                     join t in maxTapList on a.MaAnime equals t.MaAnime
                     select new
                     {
                         a.MaAnime,
                         a.Anime,
                         a.Anh,
                         a.Lp,
                         Tap = t.MaxTap
                     }).ToList();


            return Ok(c);
        }
        [Route("topviewweek")]
        [HttpGet]
        public IActionResult tvw()
        {
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var topanimetoweek = from v in db.TbViews
                        join tp in db.TbTapPhims on v.MaTp equals tp.MaTp
                        join a in db.TbAnimes on tp.MaAnime equals a.MaAnime
                        where v.NgayXem >= startOfWeek
                        group a by new { a.MaAnime, a.Anime, a.Anh,a.Lp } into g
                        orderby g.Count() descending
                        select new
                        {
                            MaAnime = g.Key.MaAnime,
                            Anime = g.Key.Anime,
                            Anh = g.Key.Anh,
                            Lp = g.Key.Lp,
                            TotalViews = g.Count()
                        };
            var top5 = topanimetoweek.Take(5).ToList();
            var maAnimeList = top5.Select(a => a.MaAnime).ToList();

            var maxTapList = (from tp in db.TbTapPhims
                              where maAnimeList.Contains(tp.MaAnime)
                              group tp by tp.MaAnime into g
                              select new { MaAnime = g.Key, MaxTap = g.Max(tp => tp.Tap) }
                            ).ToList();

            var c = (from a in top5
                     join t in maxTapList on a.MaAnime equals t.MaAnime
                     select new
                     {
                         a.MaAnime,
                         a.Anime,
                         a.Anh,
                         a.Lp,
                         Tap = t.MaxTap
                     }).ToList();

            return Ok(c);
        }
        [Route("topviewmonth")]
        [HttpGet]
        public IActionResult topviewmonth()
        {
            DateTime startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var query = from v in db.TbViews
                        join tp in db.TbTapPhims on v.MaTp equals tp.MaTp
                        join a in db.TbAnimes on tp.MaAnime equals a.MaAnime
                        where v.NgayXem >= startOfMonth
                        group a by new { a.MaAnime, a.Anime, a.Anh,a.Lp } into g
                        orderby g.Count() descending
                        select new
                        {
                            MaAnime = g.Key.MaAnime,
                            Anime = g.Key.Anime,
                            Anh = g.Key.Anh,
                            Lp = g.Key.Lp,
                            TotalViews = g.Count()
                        };
            var top5 = query.Take(5).ToList();
            var maAnimeList = top5.Select(a => a.MaAnime).ToList();

            var maxTapList = (from tp in db.TbTapPhims
                              where maAnimeList.Contains(tp.MaAnime)
                              group tp by tp.MaAnime into g
                              select new { MaAnime = g.Key, MaxTap = g.Max(tp => tp.Tap) }
                            ).ToList();

            var c = (from a in top5
                     join t in maxTapList on a.MaAnime equals t.MaAnime
                     select new
                     {
                         a.MaAnime,
                         a.Anime,
                         a.Anh,
                         a.Lp,
                         Tap = t.MaxTap
                     }).ToList();
            return Ok(c);
        }
        [Route("topviewyear")]
        [HttpGet]
        public IActionResult topviewyear()
        {
            DateTime startOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var query = from v in db.TbViews
                        join tp in db.TbTapPhims on v.MaTp equals tp.MaTp
                        join a in db.TbAnimes on tp.MaAnime equals a.MaAnime
                        where v.NgayXem >= startOfYear
                        group a by new { a.MaAnime, a.Anime, a.Anh ,a.Lp } into g
                        orderby g.Count() descending
                        select new
                        {
                            MaAnime = g.Key.MaAnime,
                            Anime = g.Key.Anime,
                            Anh = g.Key.Anh,
                            Lp = g.Key.Lp,
                            TotalViews = g.Count()
                        };
            var top5 = query.Take(5).ToList();
            var maAnimeList = top5.Select(a => a.MaAnime).ToList();

            var maxTapList = (from tp in db.TbTapPhims
                              where maAnimeList.Contains(tp.MaAnime)
                              group tp by tp.MaAnime into g
                              select new { MaAnime = g.Key, MaxTap = g.Max(tp => tp.Tap) }
                            ).ToList();

            var c = (from a in top5
                     join t in maxTapList on a.MaAnime equals t.MaAnime
                     select new
                     {
                         a.MaAnime,
                         a.Anime,
                         a.Anh,
                         a.Lp,
                         Tap = t.MaxTap
                     }).ToList();
            return Ok(c);
        }
        [HttpGet]
        [Route("paymentinfo")]
        public IActionResult GetPaymentInfo(string option)
        {
            var a = db.TbVips.FirstOrDefault(x=>x.LoaiVip == option);
            return Ok(a);
        }
    }
}
