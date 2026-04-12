using CostaRicaMusicDAL.Entidades;
namespace CostaRicaMusicDAL.Repositories;
public interface IAlbumRepository
{
    IEnumerable<Album> GetAll();
    Album? GetById(int id);
}
