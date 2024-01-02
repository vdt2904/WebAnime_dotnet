using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using WebAnime.Models;

namespace WebAnime.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("create")]
    [Route("create/create")]
    public class CreateController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        #region CreateAnime
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        // Thêm anime
        [Route("CreateAnime")]
        [HttpGet]
        public IActionResult CreateAnime()
        {
            var b = db.TbAnimes.ToList();
            bool Lp = false;
            ViewBag.Lp = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "True", Value = "True" },
                new SelectListItem { Text = "False", Value = "False" }
            }, "Value", "Text", Lp);
            ViewBag.MaHp = new SelectList(db.TbHangPhims.ToList(), "MaHp", "TenHangPhim");
            ViewBag.MaLp = new SelectList(db.TbLoaiPhims.ToList(), "MaLp", "LoaiPhim");
            if (b.Any())
            {
                var a = db.TbAnimes.Max(x => x.MaAnime);
                var nextMaAnime = maTtd(a);
                var newAnime = new TbAnime() { MaAnime = nextMaAnime };
                return View(newAnime);
            }
            else
            {
                var newAnime = new TbAnime() { MaAnime = "AE0001" };
                return View(newAnime);
            }
        }
        [Route("CreateAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnime(TbAnime ani, IFormFile file)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                var c = db.TbAnimes.FirstOrDefault(x => x.Anime == ani.Anime);
                if (c != null)
                {
                    return View(ani);
                }
                db.TbAnimes.Add(ani);
                db.SaveChanges();

                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    fileName = $"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(/*Directory.GetCurrentDirectory(),*/ "wwwroot/Layout/img/anime", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ani.Anh = fileName;
                    db.SaveChanges();
                }

                return RedirectToAction("QLPhim", "Show");
            }
            bool Lp = false;
            ViewBag.Lp = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "True", Value = "True" },
                new SelectListItem { Text = "False", Value = "False" }
            }, "Value", "Text", Lp);
            ViewBag.MaHp = new SelectList(db.TbHangPhims.ToList(), "MaHp", "TenHangPhim");
            ViewBag.MaLp = new SelectList(db.TbLoaiPhims.ToList(), "MaLp", "LoaiPhim");
            var b = db.TbAnimes.ToList();
            if (b.Any())
            {
                var a = db.TbAnimes.Max(x => x.MaAnime);
                var nextMaAnime = maTtd(a);
                var newAnime = new TbAnime() { MaAnime = nextMaAnime };
                return View(newAnime);
            }
            else
            {
                var newAnime = new TbAnime() { MaAnime = "AE0001" };
                return View(newAnime);
            }
        }
        #endregion
        #region CreateTLAnime
        [Route("CreateTLAnime")]
        [HttpGet]
        public IActionResult CreateTLAnime()
        {
            var b = db.TbTheLoais.ToList();
            if (b.Any())
            {
                var a = db.TbTheLoais.Max(x => x.MaTl);
                var nextMa = maTtd(a);
                var moi = new TbTheLoai() { MaTl = nextMa };
                return View(moi);
            }
            else
            {
                var moi = new TbTheLoai() { MaTl = "TL0001" };
                return View(moi);
            }
        }
        [Route("CreateTLAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTLAnime(TbTheLoai ani)
        {
            if (ModelState.IsValid)
            {
                var c = db.TbTheLoais.FirstOrDefault(x => x.TheLoai == ani.TheLoai);
                if (c != null)
                {
                    return View(ani);
                }
                db.TbTheLoais.Add(ani);
                db.SaveChanges();
                return RedirectToAction("TLAnime", "Show");
            }
            var b = db.TbAnimes.ToList();
            if (b.Any())
            {
                var a = db.TbTheLoais.Max(x => x.MaTl);
                var nextMa = maTtd(a);
                var moi = new TbTheLoai() { MaTl = nextMa };
                return View(moi);
            }
            else
            {
                var moi = new TbTheLoai() { MaTl = "TL0001" };
                return View(moi);
            }
        }
        #endregion
        #region CreateHPAnime
        [Route("CreateHPAnime")]
        [HttpGet]
        public IActionResult CreateHPAnime()
        {
            var b = db.TbHangPhims.ToList();
            if (b.Any())
            {
                var a = db.TbHangPhims.Max(x => x.MaHp);
                var nextMa = maTtd(a);
                var moi = new TbHangPhim() { MaHp = nextMa };
                return View(moi);
            }
            else
            {
                var moi = new TbHangPhim() { MaHp = "HP0001" };
                return View(moi);
            }
        }
        [Route("CreateHPAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHPAnime(TbHangPhim ani)
        {
            if (ModelState.IsValid)
            {
                var c = db.TbHangPhims.FirstOrDefault(x => x.TenHangPhim == ani.TenHangPhim);
                if (c != null)
                {
                    return View(ani);
                }
                db.TbHangPhims.Add(ani);
                db.SaveChanges();
                return RedirectToAction("HPAnime", "Show");
            }
            var b = db.TbHangPhims.ToList();
            if (b.Any())
            {
                var a = db.TbHangPhims.Max(x => x.MaHp);
                var nextMa = maTtd(a);
                var moi = new TbHangPhim() { MaHp = nextMa };
                return View(moi);
            }
            else
            {
                var moi = new TbHangPhim() { MaHp = "HP0001" };
                return View(moi);
            }
        }
        #endregion
        #region CreateAnimeTL
        [Route("CreateAnimeTL")]
        [HttpGet]
        public IActionResult CreateAnimeTL()
        {
            ViewBag.MaTl = new SelectList(db.TbTheLoais.ToList(), "MaTl", "TheLoai");
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View();
        }
        [Route("CreateAnimeTL")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAnimeTL(TbTlanime ani)
        {
            if (ModelState.IsValid)
            {
                var c = db.TbTlanimes.FirstOrDefault(x => x.MaAnime == ani.MaAnime && x.MaTl == ani.MaTl);
                if (c != null)
                {
                    ViewBag.MaTl = new SelectList(db.TbTheLoais.ToList(), "MaTl", "TheLoai");
                    ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
                    return View(ani);
                }
                db.TbTlanimes.Add(ani);
                db.SaveChanges();
                return RedirectToAction("AnimeTL", "Show");
            }
            ViewBag.MaTl = new SelectList(db.TbTheLoais.ToList(), "MaTl", "TheLoai");
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View(ani);
        }
        #endregion
        #region CreateBlog
        [Route("CreateBlog")]
        [HttpGet]
        public IActionResult CreateBlog()
        {
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            ViewBag.IdBlog = new SelectList(db.TbOurBlogs.ToList(), "Idblog", "TenBlog");
            return View();
        }
        [Route("CreateBlog")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlog(TbBlog ani, IFormFile video)
        {
            ModelState.Remove("video");
            if (ModelState.IsValid)
            {
                var c = db.TbBlogs.FirstOrDefault(x => x.MaAnime == ani.MaAnime && x.Idblog == ani.Idblog);
                if (c != null)
                {
                    ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
                    ViewBag.IdBlog = new SelectList(db.TbOurBlogs.ToList(), "Idblog", "TenBlog");
                    return View(ani);
                }
                db.TbBlogs.Add(ani);
                db.SaveChanges();
                if (video != null && video.Length > 0)
                {
                    var vidoeName = Path.GetFileNameWithoutExtension(video.FileName);
                    var fileExtension = Path.GetExtension(video.FileName);
                    vidoeName = $"{vidoeName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/img/blog/Trailer", vidoeName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await video.CopyToAsync(stream);
                    }
                    ani.Trailer = vidoeName;
                    db.SaveChanges();
                }
                return RedirectToAction("Blog", "Show");
            }
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            ViewBag.IdBlog = new SelectList(db.TbOurBlogs.ToList(), "Idblog", "TenBlog");
            return View();
        }
        #endregion
        #region CreateLPAnime
        [Route("CreateLPAnime")]
        [HttpGet]
        public IActionResult CreateLPAnime()
        {
            var b = db.TbLoaiPhims.ToList();
            if (b.Any())
            {
                var a = db.TbLoaiPhims.Max(x => x.MaLp);
                var nextMa = maTtd(a);
                var moi = new TbLoaiPhim() { MaLp = nextMa };
                return View(moi);
            }
            else
            {
                var moi = new TbLoaiPhim() { MaLp = "LP0001" };
                return View(moi);
            }
        }
        [Route("CreateLPAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLPAnime(TbLoaiPhim ani)
        {
            if (ModelState.IsValid)
            {
                var c = db.TbLoaiPhims.FirstOrDefault(x => x.LoaiPhim == ani.LoaiPhim);
                if (c != null)
                {
                    return View(ani);
                }
                db.TbLoaiPhims.Add(ani);
                db.SaveChanges();
                return RedirectToAction("LPAnime", "Show");
            }
            var b = db.TbLoaiPhims.ToList();
            if (b.Any())
            {
                var a = db.TbLoaiPhims.Max(x => x.MaLp);
                var nextMa = maTtd(a);
                var moi = new TbLoaiPhim() { MaLp = nextMa };
                return View(moi);
            }
            else
            {
                var moi = new TbLoaiPhim() { MaLp = "LP0001" };
                return View(moi);
            }
        }
        #endregion
        #region CreateTPAnime
        [Route("CreateTPAnime")]
        [HttpGet]
        public IActionResult CreateTPAnime()
        {
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            var b = db.TbTapPhims.ToList();
            if (b.Any())
            {
                var a = db.TbTapPhims.Max(x => x.MaTp);
                var nextMaAnime = maTtd(a);
                var newAnime = new TbTapPhim() { MaTp = nextMaAnime };
                return View(newAnime);
            }
            else
            {
                var newAnime = new TbTapPhim() { MaTp = "T00001" };
                return View(newAnime);
            }
        }
        [Route("CreateTPAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTPAnime(TbTapPhim ani, IFormFile video, IFormFile file)
        {
            ModelState.Remove("video");
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                var c = db.TbTapPhims.FirstOrDefault(x => x.MaAnime == ani.MaAnime && x.Tap == ani.Tap);
                if (c != null)
                {
                    return View(ani);
                }
                db.TbTapPhims.Add(ani);
                db.SaveChanges();
                ani.Views = 0;
                ani.Comments = 0;
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
                if (video != null && video.Length > 0)
                {
                    var vidoeName = Path.GetFileNameWithoutExtension(video.FileName);
                    var fileExtension = Path.GetExtension(video.FileName);
                    vidoeName = $"{vidoeName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Layout/videos", vidoeName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await video.CopyToAsync(stream);
                    }
                    ani.Video = vidoeName;
                    db.SaveChanges();
                }

                return RedirectToAction("TPAnime", "Show");
            }
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            var b = db.TbTapPhims.ToList();
            if (b.Any())
            {
                var a = db.TbTapPhims.Max(x => x.MaTp);
                var nextMaAnime = maTtd(a);
                var newAnime = new TbTapPhim() { MaTp = nextMaAnime };
                return View(newAnime);
            }
            else
            {
                var newAnime = new TbTapPhim() { MaTp = "T00001" };
                return View(newAnime);
            }
        }
        #endregion
        #region CreateLoaiVip
        [Route("CreateLoaiVip")]
        [HttpGet]
        public IActionResult CreateLoaiVip()
        {
            return View();
        }
        [Route("CreateLoaiVip")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLoaiVip(TbVip vp)
        {
            if (ModelState.IsValid)
            {
                db.TbVips.Add(vp);
                db.SaveChanges();
                return RedirectToAction("LoaiVip", "show");
            }
            return View(vp);
        }
        #endregion
        #region Createblogdetail
        [Route("Createblogdetail")]
        [HttpGet]
        public IActionResult Createblogdetail()
        {
            return View();
        }
        [Route("Createblogdetail")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createblogdetail(TbOurBlog ani, IFormFile file)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                db.TbOurBlogs.Add(ani);
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

                return RedirectToAction("BlogDetail", "Show");
            }
            return View(ani);
        }
        #endregion
        #region CreateWAnime
        [Route("CreateWAnime")]
        [HttpGet]
        public IActionResult CreateWAnime()
        {
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View();
        }
        [Route("CreateWAnime")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWAnime(TbWorth vp)
        {
            if (ModelState.IsValid)
            {
                var a = db.TbWorths.FirstOrDefault(x => x.MaAnime == vp.MaAnime);
                if (a != null)
                {
                    ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
                    return View(vp);
                }
                db.TbWorths.Add(vp);
                db.SaveChanges();
                return RedirectToAction("WAnime", "show");
            }
            ViewBag.MaAnime = new SelectList(db.TbAnimes.ToList(), "MaAnime", "Anime");
            return View(vp);
        }
        #endregion
        // tăng mã tự động
        private string maTtd(string a)
        {
            Match match = Regex.Match(a, @"\d+");
            int soMa = int.Parse(match.Value);
            soMa++;
            string soMaString = soMa.ToString().PadLeft(match.Value.Length, '0');
            string maMoi = a.Substring(0, match.Index) + soMaString;
            return maMoi;
        }
    }
}
