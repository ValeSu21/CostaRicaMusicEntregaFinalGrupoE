using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using CostaRicaMusicDAL.Entidades;
using CostaRicaMusicDAL.Repositories;
namespace CostaRicaMusicBLL.Services;
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repo;
    public AuthService(IUsuarioRepository repo) => _repo = repo;
    public Usuario? ValidateUser(LoginDto dto) => _repo.GetByCredentials(dto.Email, dto.Password);
}
