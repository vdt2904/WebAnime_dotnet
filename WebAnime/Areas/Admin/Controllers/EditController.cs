using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAnime.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAnime.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("edit")]
    [Route("edit/edit")]
    public class EditController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        [Route("")]
        #region editAnime
        [Route("editAnime")]
        [HttpGet]
        public IActionResult editAnime(string MaAnime)
        {
            bool Lp = false;
            ViewBag.Lp = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "True", Value = "True" },
                new SelectListItem { Text = "False", Value = "False" }
            }, "Value", "Text", Lp);
            ViewBag.MaHp = new SelectList(db.TbHangPhims.ToList(), "MaHp", "TenHangPhim");
            ViewBag.MaLp = new SelectList(db.TbLoaiPhims.ToList(), "MaLp", "LoaiPhim");
            var ani = db.TbAnimes.Find(MaAnime);
            return View(ani);
        }
        [Route("editAnime1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editAnime(TbAnime ani, IFormFile file)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                db.Entry(ani).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = $"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/img/anime", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ani.Anh = fileName;
                    db.SaveChanges();
                }
                return RedirectToAction("QLPhim", "Show");
            }
            return View(ani);
        }
        #endregion
        #region editTLAnime
        [Route("editTLAnime")]
        public IActionResult editTLAnime(string ma)
        {
            var tl = db.TbTheLoais.Find(ma);
            return View(tl);
        }
        [Route("editTLAnime1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editTLAnime(TbTheLoai tl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TLAnime", "SHow");
            }
            return View(tl);
        }
        #endregion
        #region editHPAnime
        [Route("editHPAnime")]
        public IActionResult editHPAnime(string ma)
        {
            var hp = db.TbHangPhims.Find(ma);
            return View(hp);
        }
        [Route("editHPAnime1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editHPAnime(TbHangPhim hp)
        {
            if(ModelState.IsValid) {
                db.Entry(hp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HPAnime", "SHow");
            }
            return View(hp);
        }
        #endregion
        #region editAnimeTL
        [Route("editAnimeTL")]
        public IActionResult editAnimeTL(int ma)
        {
            ViewBag.MaTl = new SelectList(db.TbTheLoais.ToList(), "MaTl", "TheLoai");
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            var hp = db.TbTlanimes.Find(ma);
            return View(hp);
        }
        [Route("editAnimeTL1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editAnimeTL(TbTlanime hp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AnimeTL", "SHow");
            }
            ViewBag.MaTl = new SelectList(db.TbTheLoais.ToList(), "MaTl", "TheLoai");
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View(hp);
        }
        #endregion
        #region editBlog
        [Route("editBlog")]
        public IActionResult editBlog(int ma)
        {
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            ViewBag.IdBlog = new SelectList(db.TbOurBlogs.ToList(), "Idblog", "TenBlog");
            var hp = db.TbBlogs.Find(ma);
            return View(hp);
        }
        [Route("editBlog1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editBlog(TbBlog hp, IFormFile video1)
        {
            ModelState.Remove("video1");
            if (ModelState.IsValid)
            {
                var c = db.TbBlogs.FirstOrDefault(x => x.MaAnime == hp.MaAnime && x.Idblog == hp.Idblog);
                if (c != null)
                {
                    ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
                    ViewBag.IdBlog = new SelectList(db.TbOurBlogs.ToList(), "Idblog", "TenBlog");
                    return View(hp);
                }
                db.Entry(hp).State = EntityState.Modified;
                db.SaveChanges();
                if (video1 != null && video1.Length > 0)
                {
                    var vidoeName = Path.GetFileNameWithoutExtension(video1.FileName);
                    var fileExtension = Path.GetExtension(video1.FileName);
                    vidoeName = $"{vidoeName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/img/blog/Trailer", vidoeName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await video1.CopyToAsync(stream);
                    }
                    hp.Trailer = vidoeName;
                    db.SaveChanges();
                }
                return RedirectToAction("Blog", "SHow");
            }
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            ViewBag.IdBlog = new SelectList(db.TbOurBlogs.ToList(), "Idblog", "TenBlog");
            return View(hp);
        }
        #endregion
        #region editLPAnime
        [Route("editLPAnime")]
        public IActionResult editLPAnime(string ma)
        {
            var hp = db.TbLoaiPhims.Find(ma);
            return View(hp);
        }
        [Route("editLPAnime1")]
        public IActionResult editLPAnime(TbLoaiPhim hp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LPAnime", "SHow");
            }
            return View(hp);
        }
        #endregion
        #region editTPAnime
        [Route("editTPAnime")]
        public IActionResult editTPAnime(string ma)
        {
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            var ani = db.TbTapPhims.Find(ma);
            return View(ani);
        }
        [Route("editTPAnime1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editTPAnime(TbTapPhim ani, IFormFile video1, IFormFile file)
        {
            ModelState.Remove("video1");
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                db.Entry(ani).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = $"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/videos", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ani.AnhVideo = fileName;
                    db.SaveChanges();
                }
                if (video1 != null && video1.Length > 0)
                {
                    var vidoeName = Path.GetFileNameWithoutExtension(video1.FileName);
                    var fileExtension = Path.GetExtension(video1.FileName);
                    vidoeName = $"{vidoeName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/videos", vidoeName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await video1.CopyToAsync(stream);
                    }
                    ani.Video = vidoeName;
                    db.SaveChanges();
                }
                return RedirectToAction("TPAnime", "Show");
            }
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View(ani);
        }
        #endregion
        #region editLoaiVip
        [Route("editLoaiVip")]
        [HttpGet]
        public IActionResult editLoaiVip(string ma)
        {
            var vp = db.TbVips.Find(ma);
            return View(vp);
        }
        [Route("editLoaiVip")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editLoaiVip(TbVip vp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LoaiVip", "show");
            }
            return View(vp);
        }
        #endregion
        #region editBlogDetail
        [Route("editBlogDetail")]
        [HttpGet]
        public IActionResult editBlogDetail(int id)
        {
            var ani = db.TbOurBlogs.Find(id);
            return View(ani);
        }
        [Route("editBlogDetail")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editBlogDetail(TbOurBlog ani, IFormFile file)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                db.Entry(ani).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = $"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/img/blog", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ani.Anh = fileName;
                    db.SaveChanges();
                }
                return RedirectToAction("blogdetail", "Show");
            }
            return View(ani);
        }
        #endregion
        #region editWAnime
        [Route("editWAnime")]
        [HttpGet]
        public IActionResult editWAnime(int ma)
        {
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            var vp = db.TbWorths.Find(ma);
            return View(vp);
        }
        [Route("editWAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult editWAnime(TbWorth vp)
        {
            if (ModelState.IsValid)
            {
                var a = db.TbWorths.FirstOrDefault(x => x.MaAnime == vp.MaAnime);
                if(a!= null)
                {
                    ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
                    return View(vp);
                }
                db.Entry(vp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("wanime", "show");
            }
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View(vp);
        }
        #endregion
    }
}
