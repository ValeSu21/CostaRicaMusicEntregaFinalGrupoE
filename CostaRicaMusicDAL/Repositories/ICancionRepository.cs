using CostaRicaMusicDAL.Entidades;
namespace CostaRicaMusicDAL.Repositories;
public interface ICancionRepository
{
    IEnumerable<Cancion> GetAll();
    Cancion? GetById(int id);
}
