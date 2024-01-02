using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace WebAnime.Models
{
    public class MyJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public MyJobService(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        public void UpdateNguoiDungLoaiND()
        {
            // Do your job here
            using (var db = new QlAnimeContext())
            {
                db.Database.ExecuteSqlRaw("update tb_NguoiDung set tb_NguoiDung.LoaiND = 0 where tb_NguoiDung.LoaiND = 1 and GETDATE() > (select top 1 tb_HoaDon.NgayHetHan from tb_HoaDon where tb_HoaDon.MaND = tb_NguoiDung.MaND order by tb_HoaDon.SoHD desc)");
                db.SaveChanges();
                _backgroundJobClient.Schedule(() => UpdateNguoiDungLoaiND(), TimeSpan.FromMinutes(1));
            }
        }
    }
}
