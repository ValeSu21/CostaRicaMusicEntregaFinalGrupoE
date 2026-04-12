using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using CostaRicaMusicDAL.Repositories;
namespace CostaRicaMusicBLL.Services;
public class ArtistaService : IArtistaService
{
    private readonly IArtistaRepository _repo;
    public ArtistaService(IArtistaRepository repo) => _repo = repo;
    public ArtistaDto? GetById(int id)
    {
        var x = _repo.GetById(id);
        if (x is null) return null;
        return new ArtistaDto
        {
            IdArtista = x.IdArtista,
            Nombre = x.Nombre,
            Genero = x.Genero,
            Pais = x.Pais,
            Biografia = x.Biografia,
            ImagenUrl = x.ImagenUrl
        };
    }
}
