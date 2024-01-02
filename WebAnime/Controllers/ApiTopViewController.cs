using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using WebAnime.Models;
using System.Text.Json;

namespace WebAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTopViewController : ControllerBase
    {
        QlAnimeContext db = new QlAnimeContext();
        [HttpPut]
        public IActionResult dmk(string mand, string mkc, string mkm, string nmkm)
        {
            var tk = db.TbNguoiDungs.FirstOrDefault(x => x.MaNd == mand && x.Pasword == MD5Hash(mkc));
            if (tk == null)
            {
                return BadRequest("Sai mật khẩu: " + mkc);
            }
            if (nmkm != mkm)
            {
                return BadRequest("Sai mật khẩu mới");
            }
            tk.Pasword = MD5Hash(mkm);
            db.SaveChanges();
            return Ok("Thay đổi mật khẩu thành công");

        }
        [Route("getcmt")]
        [HttpGet]
        public IActionResult getCmt(string ma)
        {
            var comments = db.TbComments
                .Join(db.TbNguoiDungs, c => c.MaNd, nd => nd.MaNd, (c, nd) => new { c, nd })
                .Where(cn => cn.c.MaTp == ma)
                .OrderByDescending(cn => cn.c.Id)
                .Select(cn => new { cn.nd.TenNguoiDung, cn.c.Comment, cn.c.NgayComent })
                .ToList();
            var listcmt = comments.Count();
            var result = new
            {
                TotalCount = listcmt,
                Items = comments
            };
            return Ok(result);
        }
        [Route("getcmtbypage")]
        [HttpGet]
        public IActionResult getPagination([Range(1, 10)] int pageSize, [Range(1, int.MaxValue)] int pageNumber,string ma)
        {
            var comments = db.TbComments
                        .Join(db.TbNguoiDungs, c => c.MaNd, nd => nd.MaNd, (c, nd) => new { c, nd })
                        .Where(cn => cn.c.MaTp == ma)
                        .OrderByDescending(cn => cn.c.Id)
                        
                        .Skip((pageNumber-1)*pageSize)
                        .Take(pageSize)
                        .Select(cn => new { cn.nd.TenNguoiDung, cn.c.Comment, cn.c.NgayComent })
                        .ToList();
            var result = new
            {
                Items = comments,
            };
            return Ok(result);
        }
        [Route("insertcmt")]
        [HttpPost]
        public IActionResult insertcom(string mtp, string mnd, string cm)
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sql = "INSERT INTO tb_comment (MaNd, MaTp, Comment, NgayComent) VALUES ('" + mnd + "', '" + mtp + "', '" + cm + "', '" + formattedDate + "')";
            db.Database.ExecuteSqlRaw(sql);
            db.SaveChanges();
            return Ok("Comment thành công");
        }
        [Route("TangView")]
        [HttpPost]
        public IActionResult Halfway([FromBody] JObject data)
        {
            var mnd = data["mnd"].ToString();
            var mtp = data["mtp"].ToString();
            var a = db.TbTapPhims.Find(mtp);
            a.Views += 1;
            db.SaveChanges();
            var b = new TbView
            {
                MaTp = mtp,
                MaNd = mnd,
                NgayXem = DateTime.Now
            };
            db.TbViews.Add(b);
            db.SaveChanges();
            return new JsonResult(new { success = true });
        }
        [Route("phimnenxem")]
        [HttpGet]
        public IActionResult pnx()
        {
            var result = (from worth in db.TbWorths
                          join anime in db.TbAnimes on worth.MaAnime equals anime.MaAnime
                          orderby worth.Id descending
                          select new { worth.Id, anime.MaAnime, anime.Anime, anime.Anh, anime.ThongTin })
                         .Take(3)
                         .ToList();
            var maAnimeList = result.Select(a => a.MaAnime).ToList();
            var result1 = from anime in db.TbAnimes
                         join tlAnime in db.TbTlanimes on anime.MaAnime equals tlAnime.MaAnime
                         join theLoai in db.TbTheLoais on tlAnime.MaTl equals theLoai.MaTl
                         where maAnimeList.Contains(tlAnime.MaAnime)
                         group theLoai by anime.MaAnime into g
                         orderby g.Max(x => x.TheLoai) descending
                         select new { MaAnime = g.Key, TopGenre = g.Max(x => x.TheLoai) };
            var c = (from a in result
                     join b in result1 on a.MaAnime equals b.MaAnime
                     orderby a.Id descending
                     select new
                     {
                         a.Id,
                         a.MaAnime,
                         a.Anime,
                         a.Anh,
                         a.ThongTin,
                         b.TopGenre
                     }).ToList(); 
            return Ok(c);
        }
        private string MD5Hash(string input)
        {
            using (MD5 md5hash = MD5.Create())
            {
                byte[] data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
