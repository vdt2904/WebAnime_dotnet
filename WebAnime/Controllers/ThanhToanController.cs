using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebAnime.Models;

namespace WebAnime.Controllers
{
    public class ThanhToanController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        [HttpGet]
        public IActionResult ThanhToanP()
        {
            var b = db.TbNguoiDungs.FirstOrDefault(x => x.MaNd == HttpContext.Session.GetString("DangNhap"));
            if(b == null || HttpContext.Session.GetString("DangNhap") == null)
            {
                return RedirectToAction("DangNhap", "HomeAccess");
            }
            if(b.LoaiNd == 1)
            {
                return RedirectToAction("ThonTinNd", "HomeAccess", new { ma = HttpContext.Session.GetString("DangNhap") });

            }
            return View(b);
        }
        [HttpPost]
        public IActionResult ThanhToanP(string em , string lv)
        {
            var a = db.TbNguoiDungs.FirstOrDefault(x => x.Email == em);
            var b = db.TbVips.FirstOrDefault(x => x.LoaiVip == lv);
            var nds = db.TbHoaDons.ToList();
            string ma = "";
            if (nds.Count > 0)
            {
                var hd = db.TbHoaDons.Max(x => x.SoHd);
                ma = maTtd(em.ToString());
            }
            else
            {
                ma = "HD0001";
            }
            var c = new TbHoaDon
            {
                SoHd = ma,
                MaNd = a.MaNd,
                LoaiVip = lv,
                NgayMua = DateTime.Now,
                NgayHetHan = DateTime.Now.AddMonths(b.ThoiGianSd??0)
            };
            db.TbHoaDons.Add(c);
            db.SaveChanges();
            a.LoaiNd = 1;
            db.SaveChanges();
            var d = db.TbDoanhThus.FirstOrDefault(x => x.Nam == c.NgayMua.Value.Year && x.Thang == c.NgayMua.Value.Month);
            if(d!= null)
            {
                d.TongTien += b.Gia;
            }
            else
            {
                var e = new TbDoanhThu
                {
                    Thang = c.NgayMua.Value.Month,
                    Nam = c.NgayMua.Value.Year,
                    TongTien = 0
                };
                db.TbDoanhThus.Add(e);
                db.SaveChanges();
                e.TongTien += b.Gia;
                db.SaveChanges();

            }


            return RedirectToAction("index","home");
        }

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
