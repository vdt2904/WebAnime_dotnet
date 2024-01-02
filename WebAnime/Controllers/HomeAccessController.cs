using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebAnime.Models;
using MimeKit;
using System.Net.Mail;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebAnime.Controllers
{
    public class HomeAccessController : Controller
    {
        

        QlAnimeContext db = new QlAnimeContext();
        #region Đăng nhập
        public IActionResult DangNhap()
        {
            if (HttpContext.Session.GetString("DangNhap") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult DangNhap(TbNguoiDung ad)
        {
            TempData["MaND"] = "";
            TempData["username"] = "";
            if (HttpContext.Session.GetString("DangNhap") == null)
            {
                string pass = "";
                pass = MD5Hash(ad.Pasword);
                var u = db.TbNguoiDungs.Where(x => x.Email == ad.Email && x.Pasword == pass).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("DangNhap", u.MaNd.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(ad);
        }
        #endregion
        #region Đăng ký
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(TbNguoiDung nd)
        {
            TempData["error"] = "";
            var dk = db.TbNguoiDungs.FirstOrDefault(x => x.Email == nd.Email);
            if (dk != null)
            {
                TempData["error"] = "Email đã tồn tại";
                return View(nd);
            }
            var nds = db.TbNguoiDungs.ToList();
            string ma = "";
            if (nds.Count > 0)
            {
                var em = db.TbNguoiDungs.Max(x => x.MaNd);
                ma = maTtd(em.ToString());
            }
            else
            {
                ma = "ND0001";
            }
            nd.MaNd = ma;
            db.TbNguoiDungs.Add(nd);
            db.SaveChanges();
            nd.Pasword = MD5Hash(nd.Pasword);
            db.SaveChanges();
            nd.LoaiNd = 0;
            db.SaveChanges();
            return RedirectToAction("DangNhap", "HomeAccess", new { nd });
        }
        #endregion
        #region ThongTinNd
        [HttpGet]
        public IActionResult ThonTinNd(string ma)
        {
            var tt = db.TbNguoiDungs.FirstOrDefault(x => x.MaNd == ma);
            return View(tt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThonTinNd(TbNguoiDung nd)
        {
            if (ModelState.IsValid)
            {
                TempData["MaND"] = nd.MaNd.ToString();
                db.Entry(nd).State = EntityState.Modified;
                db.SaveChanges();
                return View(nd);
            }
            return View();
        }
        #endregion
        #region Đổi mật khẩu
        public IActionResult DMK(string ma)
        {
            var tt = db.TbNguoiDungs.FirstOrDefault(x => x.MaNd == ma);
            return View(tt);
        }
        #endregion
        #region Quên mật khẩu
        private readonly IConfiguration _configuration;

        public HomeAccessController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult QuenMk()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> QuenMk(string em)
        {
            var emailConfig = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
            var user = db.TbNguoiDungs.FirstOrDefault(x => x.Email == em);
            if (user == null)
            {
                ViewBag.error = "Email không tồn tại";
                return View();
            }
            string newPassword = GenerateRandomPassword();
            var a = new TbResetPass
            {
                MaNd = user.MaNd,
                Email = user.Email,
                ResetPass = MD5Hash(newPassword),
                Token = newPassword,
                ThoiGian = DateTime.Now
            };
            db.TbResetPasses.Add(a);
            db.SaveChanges();
            var emailMessage = new MailMessage();
            emailMessage.From = new MailAddress("vudoanhthai12b2002hla@gmail.com", "Vũ Doanh Thái");
            emailMessage.To.Add(new MailAddress(user.Email, user.TenNguoiDung));
            emailMessage.Subject = "Password Reset";
            emailMessage.Body = $"New PassWord: {a.Token} To reset your password, click on the following link: http://localhost:5153/HomeAccess/ResetPW?code={a.Token}";
            using (var client = new SmtpClient(emailConfig.Host, emailConfig.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(emailConfig.Email, emailConfig.Password);
                await client.SendMailAsync(emailMessage);
            }
            ViewBag.error = "Đã gửi mail thành công! Vui lòng kiểm tra email để biết mật khẩu mới";
            return View();
        }
        public IActionResult ResetPW(string code)
        {
            var a = db.TbResetPasses.FirstOrDefault(x => x.Token == code);
            if(a== null)
            {
                ViewBag.error = "Đường link xác nhận không hợp lệ hoặc đã hết hạn";
                return View();
            }
            var b = db.TbNguoiDungs.FirstOrDefault(x => x.MaNd == a.MaNd);
            b.Pasword = a.ResetPass;
            db.SaveChanges();
            ViewBag.error = "Đã đổi mật khẩu thành công";
            db.TbResetPasses.Remove(a);
            db.SaveChanges();
            return View();
        }
        public class EmailSettings
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string DisplayName { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public bool EnableSSL { get; set; }
        }
        private string GenerateRandomPassword()
        {
            // generate a random password of length 8
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
        #region đăng xuất
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("DangNhap");
            return RedirectToAction("dangnhap", "Homeaccess");
        }
        #endregion
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
