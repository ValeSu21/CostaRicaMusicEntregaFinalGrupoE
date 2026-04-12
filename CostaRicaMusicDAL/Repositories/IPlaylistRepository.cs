using CostaRicaMusicDAL.Entidades;
namespace CostaRicaMusicDAL.Repositories;
public interface IPlaylistRepository
{
    IEnumerable<Playlist> GetByUser(string email);
    Playlist? GetById(int id);
    Playlist Create(Playlist playlist);
    void Update(Playlist playlist);
    void Delete(int id);
}
