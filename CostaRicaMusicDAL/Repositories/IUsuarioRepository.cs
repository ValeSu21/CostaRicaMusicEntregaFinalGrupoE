using CostaRicaMusicDAL.Entidades;
namespace CostaRicaMusicDAL.Repositories;
public interface IUsuarioRepository
{
    Usuario? GetByCredentials(string email, string password);
}
