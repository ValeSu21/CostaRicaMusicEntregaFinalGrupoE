using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicDAL.Entidades;
namespace CostaRicaMusicBLL.Interfaces;
public interface IAuthService
{
    Usuario? ValidateUser(LoginDto dto);
}
