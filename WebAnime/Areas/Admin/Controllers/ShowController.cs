using WebAnime.Areas.Admin.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnime.Models;
using X.PagedList;
using WebAnime.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAnime.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    [Authentication]
    public class ShowController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        [Route("")]
        [Route("index")]

        public IActionResult Index()
        {
            DateTime date = DateTime.Now;
            var viewModel = new MyViewModel();
            ViewBag.Yearss = new SelectList(db.TbDoanhThus.Select(x => x.Nam).Distinct().OrderByDescending(x => x).ToList(), date.Year);
            viewModel.TDoanhThus = db.TbDoanhThus
                .Where(x => x.Nam == date.Year)
                .OrderBy(x => x.Thang)
                .Select(x => new BarChartViewModel { Labels = new List<string> { "Tháng: " + x.Thang.ToString() }, Data = new List<double?> { x.TongTien } })
                .ToList();
            double totalDT = db.TbDoanhThus.Where(x => x.Nam == date.Year).Sum(x => x.TongTien) ?? 0;
            viewModel.DoanhThu = totalDT.ToString("N0");
            return View(viewModel);
        }
        [Route("Index")]
        [HttpPost]
        public IActionResult Index(int year)
        {
            var viewModel = new MyViewModel();
            //doanhthu
            ViewBag.Yearss = new SelectList(db.TbDoanhThus.Select(x => x.Nam).Distinct().OrderByDescending(x => x).ToList(), year);
            viewModel.TDoanhThus = db.TbDoanhThus
                .Where(x => x.Nam == year)
                .OrderBy(x => x.Thang)
                .Select(x => new BarChartViewModel { Labels = new List<string> { "Tháng: " + x.Thang.ToString() }, Data = new List<double?> { x.TongTien } })
                .ToList();
            double totalDT = db.TbDoanhThus.Where(x => x.Nam == year).Sum(x => x.TongTien) ?? 0;
            viewModel.DoanhThu = totalDT.ToString("N0");

            return View(viewModel);
        }
        public class MyViewModel
        {
            public List<BarChartViewModel> TDoanhThus { get; set; }
            public string TongTien { get; set; }
            public string DoanhThu { get; set; }
        }
        #region QLAnime
        //Amime
        [Route("QLPhim")]
        public IActionResult QLPhim(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstAnime = db.TbAnimes.AsNoTracking().OrderByDescending(x => x.Anime);
            PagedList<TbAnime> lst = new PagedList<TbAnime>(lstAnime, pageNumber, pageSize);
            return View(lst);
        }
        //Thể loại
        [Route("TLAnime")]
        public IActionResult TLAnime(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstTl = db.TbTheLoais.AsNoTracking().OrderBy(x => x.TheLoai);
            PagedList<TbTheLoai> lst = new PagedList<TbTheLoai>(lstTl, pageNumber, pageSize);
            return View(lst);
        }
        //Hãng phim
        [Route("HPAnime")]
        public IActionResult HPAnime(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstHP = db.TbHangPhims.AsNoTracking().OrderBy(x => x.TenHangPhim);
            PagedList<TbHangPhim> lst = new PagedList<TbHangPhim>(lstHP, pageNumber, pageSize);
            return View(lst);
        }
        //Thể loại của anime
        [Route("AnimeTL")]
        public IActionResult AnimeTL(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lsttl = db.TbTlanimes.AsNoTracking().OrderBy(x => x.MaAnime);
            PagedList<TbTlanime> lst = new PagedList<TbTlanime>(lsttl, pageNumber, pageSize);
            return View(lst);
        }
        //Loại phim
        [Route("LPAnime")]
        public IActionResult LPAnime(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstLP = db.TbLoaiPhims.AsNoTracking().OrderBy(x => x.LoaiPhim);
            PagedList<TbLoaiPhim> lst = new PagedList<TbLoaiPhim>(lstLP, pageNumber, pageSize);
            return View(lst);
        }
        //Tập phim
        [Route("TPAnime")]
        public IActionResult TPAnime(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstTP = db.TbTapPhims.AsNoTracking().OrderBy(x => x.MaTp);
            PagedList<TbTapPhim> lst = new PagedList<TbTapPhim>(lstTP, pageNumber, pageSize);
            return View(lst);
        }
        // Đáng xem
        [Route("WAnime")]
        public IActionResult WAnime(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstTP = db.TbWorths.AsNoTracking().OrderBy(x => x.MaAnime);
            PagedList<TbWorth> lst = new PagedList<TbWorth>(lstTP, pageNumber, pageSize);
            return View(lst);
        }
        #endregion

        #region QLBlog
        //Blog
        [Route("Blog")]
        public IActionResult Blog(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstblog = db.TbBlogs.AsNoTracking().OrderByDescending(x => x.MaAnime);
            PagedList<TbBlog> lst = new PagedList<TbBlog>(lstblog, pageNumber, pageSize);
            return View(lst);
        }
        //Blog detail
        [Route("BlogDetail")]
        public IActionResult BlogDetail(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstblog = db.TbOurBlogs.AsNoTracking().OrderByDescending(x => x.Idblog);
            PagedList<TbOurBlog> lst = new PagedList<TbOurBlog>(lstblog, pageNumber, pageSize);
            return View(lst);
        }
        #endregion

        #region QLHoaDon
        [Route("HoaDon")]
        public IActionResult HoaDon(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstAnime = db.TbHoaDons.AsNoTracking().OrderBy(x => x.SoHd);
            PagedList<TbHoaDon> lst = new PagedList<TbHoaDon>(lstAnime, pageNumber, pageSize);
            return View(lst);
        }
        [Route("NguoiDung")]
        public IActionResult NguoiDung(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstAnime = db.TbNguoiDungs.AsNoTracking().OrderBy(x => x.MaNd);
            PagedList<TbNguoiDung> lst = new PagedList<TbNguoiDung>(lstAnime, pageNumber, pageSize);
            return View(lst);
        }
        [Route("LoaiVip")]
        public IActionResult LoaiVip(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstAnime = db.TbVips.AsNoTracking().OrderBy(x => x.LoaiVip);
            PagedList<TbVip> lst = new PagedList<TbVip>(lstAnime, pageNumber, pageSize);
            return View(lst);
        }
        #endregion
    }
}
