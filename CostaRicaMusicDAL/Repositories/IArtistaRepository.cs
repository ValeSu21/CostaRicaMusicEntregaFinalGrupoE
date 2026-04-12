using CostaRicaMusicDAL.Entidades;
namespace CostaRicaMusicDAL.Repositories;
public interface IArtistaRepository
{
    IEnumerable<Artista> GetAll();
    Artista? GetById(int id);
}
