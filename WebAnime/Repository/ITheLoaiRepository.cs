using WebAnime.Models;
namespace WebAnime.Repository
{
    public interface ITheLoaiRepository
    {
        TbTheLoai Add(TbTheLoai tl);
        TbTheLoai Update(TbTheLoai tl);
        TbTheLoai Delete(string ma);
        TbTheLoai GetTl(string ma);
        IEnumerable<TbTheLoai> GetAllTl();
    }
}
