using CostaRicaMusicBLL.DTOs;
namespace CostaRicaMusicBLL.Interfaces;
public interface IAlbumService
{
    AlbumDto? GetById(int id);
}
