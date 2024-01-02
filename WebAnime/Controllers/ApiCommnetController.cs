using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAnime.Models;

namespace WebAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCommnetController : ControllerBase
    {
        QlAnimeContext db = new QlAnimeContext();
        [HttpGet]
        public IActionResult getCmt(string ma)
        {
            var comments = db.TbComments
                .Join(db.TbNguoiDungs, c => c.MaNd, nd => nd.MaNd, (c, nd) => new { c, nd })
                .Where(cn => cn.c.MaTp == ma)
                .OrderByDescending(cn => cn.c.Id)
                .Take(5)
                .Select(cn => new { cn.nd.TenNguoiDung, cn.c.Comment })
                .ToList();
            return Ok(comments);
        }
    }
}
