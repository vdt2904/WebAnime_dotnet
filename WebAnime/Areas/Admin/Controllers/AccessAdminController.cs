using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebAnime.Models;

namespace WebAnime.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("LoginAdmin")]
    [Route("LoginAdmin/AccessAdmin")]
    public class AccessAdminController : Controller
    {
        QlAnimeContext db = new QlAnimeContext();
        [Route("")]
        [Route("Login")]

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("loginadmin") == null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Show");
            }
        }
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(WebAnime.Models.Admin ad)
        {
            TempData["username"] = "";
            if (HttpContext.Session.GetString("loginadmin") == null)
            {
                string pass = "";
                pass = MD5Hash(ad.Password);
                var u = db.Admins.Where(x => x.Username == ad.Username && x.Password == pass).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("loginadmin", u.Username.ToString());
                    TempData["username"] = HttpContext.Session.GetString("loginadmin");
                    return RedirectToAction("Index", "Show");
                }
            }
            return View(ad);
        }
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("loginadmin");
            return RedirectToAction("Login", "AccessAdmin");
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
