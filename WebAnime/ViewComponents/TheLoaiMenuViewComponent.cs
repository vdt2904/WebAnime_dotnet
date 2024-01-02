using WebAnime.Models;
using Microsoft.AspNetCore.Mvc;
using WebAnime.Repository;

namespace WebAnime.ViewComponents
{
    public class TheLoaiMenuViewComponent: ViewComponent
    {
        public readonly ITheLoaiRepository _theloai;
        public TheLoaiMenuViewComponent(ITheLoaiRepository theloai)
        {
            _theloai = theloai;
        }
        public IViewComponentResult Invoke()
        {
            var theloai = _theloai.GetAllTl().OrderBy(x => x.TheLoai);
            return View(theloai);
        }
    }
}
