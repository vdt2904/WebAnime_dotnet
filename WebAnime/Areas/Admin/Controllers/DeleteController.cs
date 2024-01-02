using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnime.Models;

namespace WebAnime.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("delete")]
    [Route("delete/delete")]
    public class DeleteController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        [Route("")]
        [Route("deleteAnime")]
        public IActionResult deleteAnime(string ma)
        {
            TempData["Message"] = "";
            var blogs = db.TbBlogs.Where(x => x.MaAnime == ma).AsEnumerable();
            if (blogs.Any())
            {
                string sql = "DELETE FROM tb_blog WHERE MaAnime = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }
            var tlani = db.TbTlanimes.Where(x => x.MaAnime == ma).AsEnumerable();
            if (tlani.Any())
            {
                string sql = "DELETE FROM tb_tlanime WHERE MaAnime = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }
            var twani = db.TbWorths.Where(x => x.MaAnime == ma).AsEnumerable();
            if (tlani.Any())
            {
                string sql = "DELETE FROM tb_worth WHERE MaAnime = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }
            var taps = db.TbTapPhims.Where(x => x.MaAnime == ma).AsEnumerable().ToList();
            if (taps.Any())
            {
                foreach(var t in taps) {
                    var vtaps = db.TbViews.Where(x => x.MaTp == t.MaTp).AsEnumerable();
                    if (tlani.Any())
                    {
                        string sql1 = "DELETE FROM tb_View WHERE Matp = {0}";
                        db.Database.ExecuteSqlRaw(sql1, t.MaTp);
                        db.SaveChanges();
                    }
                    var ctaps = db.TbComments.Where(x => x.MaTp == t.MaTp).AsEnumerable();
                    if (ctaps.Any())
                    {
                        string sql1 = "DELETE FROM tb_Comment WHERE Matp = {0}";
                        db.Database.ExecuteSqlRaw(sql1, t.MaTp);
                        db.SaveChanges();
                    }
                }
                string sql = "DELETE FROM tb_tapphim WHERE MaAnime = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }           
            db.Remove(db.TbAnimes.Find(ma));
            db.SaveChanges();
            TempData["Message"] = "Anime đã được xóa";
            return RedirectToAction("QlPhim", "show");
        }
        [Route("deleteTLAnime")]
        public IActionResult deleteTLAnime(string ma)
        {
            TempData["Message"] = "";
            var tlani = db.TbTlanimes.FirstOrDefault(x => x.MaTl == ma);
            if(tlani != null)
            {
                TempData["Message"] = "Không được xóa thể loại này!";
                return RedirectToAction("TLAnime", "show");
            }
            TempData["Message"] = "Thể loại đã được xóa!";
            db.Remove(db.TbTheLoais.Find(ma));
            db.SaveChanges();
            return RedirectToAction("TLAnime", "show");
        }
        [Route("deleteHPAnime")]
        public IActionResult deleteHPAnime(string ma)
        {
            TempData["Message"] = "";
            var ani = db.TbAnimes.FirstOrDefault(x => x.MaHp == ma);
            if (ani != null)
            {
                TempData["Message"] = "Không được xóa Hãng phim này!";
                return RedirectToAction("TLAnime", "show");
            }
            TempData["Message"] = "Hãng phim đã được xóa!";
            db.Remove(db.TbHangPhims.Find(ma));
            db.SaveChanges();
            return RedirectToAction("HPAnime", "show");
        }
        [Route("deleteAnimeTL")]
        public IActionResult deleteAnimeTL(int ma)
        {
            TempData["Message"] = "";
            TempData["Message"] = "Thể loại đã được xóa khỏi anime!";
            db.Remove(db.TbTlanimes.Find(ma));
            db.SaveChanges();
            return RedirectToAction("AnimeTL", "show");
        }
        [Route("deleteBlog")]
        public IActionResult deleteBlog(int ma)
        {
            TempData["Message"] = "";
            TempData["Message"] = "Blog đã được xóa!";
            db.Remove(db.TbBlogs.Find(ma));
            db.SaveChanges();
            return RedirectToAction("Blog", "show");
        }
        [Route("deleteLPAnime")]
        public IActionResult deleteLPAnime(string ma)
        {
            TempData["Message"] = "";
            TempData["Message"] = "Loại phim đã được xóa!";
            db.Remove(db.TbLoaiPhims.Find(ma));
            db.SaveChanges();
            return RedirectToAction("LPAnime", "show");
        }
        [Route("deleteTPAnime")]
        public IActionResult deleteTPAnime(string ma)
        {
            TempData["Message"] = "";
            var views = db.TbViews.Where(x => x.MaTp == ma).AsEnumerable();
            if (views.Any())
            {
                string sql = "DELETE FROM tb_view WHERE MaAnime = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }
            var cmts = db.TbComments.Where(x => x.MaTp == ma).AsEnumerable();
            if (cmts.Any())
            {
                string sql = "DELETE FROM tb_Comment WHERE MaAnime = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }
            TempData["Message"] = "Loại phim đã được xóa!";
            db.Remove(db.TbTapPhims.Find(ma));
            db.SaveChanges();
            return RedirectToAction("TPAnime", "show");
        }
        [Route("deleteLoaiVip")]
        public IActionResult deleteLoaiVip(string ma)
        {
            TempData["Message"] = "";
            var hd = db.TbHoaDons.FirstOrDefault(x => x.LoaiVip == ma);
            if(hd != null)
            {
                TempData["Message"] = "Loại Vip này không thể xóa";
                return View(ma);
            }
            TempData["Message"] = "Loại vip đã được xóa!";
            db.Remove(db.TbVips.Find(ma));
            db.SaveChanges();
            return RedirectToAction("LoaiVip", "show");
        }
        [Route("deleteBlogdetail")]
        public IActionResult deleteBlogdetail(int ma)
        {
            TempData["Message"] = "";
            var blogs = db.TbBlogs.Where(x => x.Idblog == ma).AsEnumerable();
            if (blogs.Any())
            {
                string sql = "DELETE FROM tb_blog WHERE idblog = {0}";
                db.Database.ExecuteSqlRaw(sql, ma);
                db.SaveChanges();
            }
            db.Remove(db.TbOurBlogs.Find(ma));
            db.SaveChanges();
            TempData["Message"] = "Blog đã được xóa";
            return RedirectToAction("blogdetail", "show");
        }
        [Route("deleteWAnime")]
        public IActionResult deleteWAnime(int ma)
        {
            TempData["Message"] = "";
            TempData["Message"] = "đã được xóa!";
            db.Remove(db.TbWorths.Find(ma));
            db.SaveChanges();
            return RedirectToAction("wanime", "show");
        }
    }
}
