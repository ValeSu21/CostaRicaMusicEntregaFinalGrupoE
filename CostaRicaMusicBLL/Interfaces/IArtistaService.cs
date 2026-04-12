using CostaRicaMusicBLL.DTOs;
namespace CostaRicaMusicBLL.Interfaces;
public interface IArtistaService
{
    ArtistaDto? GetById(int id);
}
