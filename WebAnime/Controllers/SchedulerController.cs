using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAnime.Models;

namespace WebAnime.Controllers
{
    public class SchedulerController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RunSchedularMethod()
        {
            db.Database.ExecuteSqlRaw("update tb_NguoiDung set tb_NguoiDung.LoaiND = 0 where tb_NguoiDung.LoaiND = 1 and GETDATE() > (select top 1 tb_HoaDon.NgayHetHan from tb_HoaDon where tb_HoaDon.MaND = tb_NguoiDung.MaND order by tb_HoaDon.SoHD desc)");
            db.SaveChanges();
            throw new NotImplementedException();
        }
    }
}
