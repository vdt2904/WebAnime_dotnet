using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAnime.Models;

namespace WebAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTimKiemController : ControllerBase
    {
        QlAnimeContext db = new QlAnimeContext();
        [HttpGet]
        public IActionResult timkie(string search)
        {
            string[] keywords = search.Split(' ');

            var result = db.TbAnimes
                .AsEnumerable()
                .Where(anime => keywords.Any(keyword =>
                    anime.Anime.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0))
                .Select(anime => new
                {
                    anime.MaAnime,
                    anime.Anime,
                    anime.Anh,
                    anime.TongSoTap
                })
                .ToList();
            return Ok(result);
        }
    }
}
