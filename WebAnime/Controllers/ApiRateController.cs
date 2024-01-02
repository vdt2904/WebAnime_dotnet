using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using WebAnime.Models;
using WebAnime.Models.Authentication;
using static System.Net.WebRequestMethods;

namespace WebAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiRateController : ControllerBase
    {
        QlAnimeContext db = new QlAnimeContext();
        [HttpGet]
        public IActionResult lstreview(string ma)
        {
            var reviews = db.TbReviews
                .Join(db.TbNguoiDungs, c => c.MaNd, nd => nd.MaNd, (c, nd) => new { c, nd })
                .Where(cn => cn.c.MaAnime == ma)
                .OrderByDescending(cn => cn.c.Id)
                .Select(cn => new { cn.nd.TenNguoiDung, cn.c.Review, cn.c.NgayReview,cn.c.Rate })
                .ToList();
            var c = new
            {
                TotalCount = reviews.Count(),
                LstReview = reviews
            };
            return Ok(c);
        }
        [Route("getrwbypage")]
        [HttpGet]
        public IActionResult getPagination1([Range(1, 10)] int pageSize, [Range(1, int.MaxValue)] int pageNumber, string ma)
        {
            var comments = db.TbReviews
                        .Join(db.TbNguoiDungs, c => c.MaNd, nd => nd.MaNd, (c, nd) => new { c, nd })
                        .Where(cn => cn.c.MaAnime == ma)
                        .OrderByDescending(cn => cn.c.Id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(cn => new { cn.nd.TenNguoiDung, cn.c.Review, cn.c.NgayReview,cn.c.Rate })
                        .ToList();
            var result = new
            {
                Items = comments,
            };
            return Ok(result);
        }
        [Route("gettbrate")]
        [HttpGet]
        public IActionResult gettbrate(string ma1)
        {
            var result = db.TbReviews
                .Where(r => r.MaAnime == ma1)
                .Select(r => (float?)r.Rate)
                .Average() ?? 0;


            var i = db.TbReviews.Count(x => x.MaAnime == ma1);
            var a = new
            {
                TongVote = i,
                TbRate = result
            };
            return Ok(a);
        }
        [Route("insertReview")]
        [HttpPost]
       
        public IActionResult insertReview(string mnd , string ma , string rv , int r)
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sql = "INSERT INTO tb_review (MaAnime, Mand, review, rate, ngayreview) VALUES ('" + ma + "', '" + mnd + "', N'" + rv + "', " + r + ", '" + formattedDate + "')";

            db.Database.ExecuteSqlRaw(sql);
            db.SaveChanges();
            return Ok("Comment thành công");
        }
    }
}
