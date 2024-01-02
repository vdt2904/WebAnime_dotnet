using WebAnime.Models;
namespace WebAnime.Repository
{
    public class TheLoaiRepository : ITheLoaiRepository
    {
        public readonly QlAnimeContext _context;
        public TheLoaiRepository(QlAnimeContext context)
        {
            _context = context;
        }

        public TbTheLoai Add(TbTheLoai tl)
        {
            _context.TbTheLoais.Add(tl);
            _context.SaveChanges();
            return tl;
        }

        public TbTheLoai Delete(string ma)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TbTheLoai> GetAllTl()
        {
            return _context.TbTheLoais;
        }

        public TbTheLoai GetTl(string ma)
        {
            return _context.TbTheLoais.Find(ma);
        }

        public TbTheLoai Update(TbTheLoai tl)
        {
            _context.Update(tl);
            _context.SaveChanges();
            return tl;
        }
    }
}
