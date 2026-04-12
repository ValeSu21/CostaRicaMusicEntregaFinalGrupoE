using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using CostaRicaMusicDAL.Repositories;
namespace CostaRicaMusicBLL.Services;
public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumes;
    private readonly IArtistaRepository _artistas;
    private readonly ICancionRepository _canciones;
    public AlbumService(IAlbumRepository albumes, IArtistaRepository artistas, ICancionRepository canciones)
    {
        _albumes = albumes;
        _artistas = artistas;
        _canciones = canciones;
    }
    public AlbumDto? GetById(int id)
    {
        var album = _albumes.GetById(id);
        if (album is null) return null;
        var artista = _artistas.GetById(album.IdArtista);
        return new AlbumDto
        {
            IdAlbum = album.IdAlbum,
            Titulo = album.Titulo,
            IdArtista = album.IdArtista,
            Artista = artista?.Nombre ?? "N/D",
            Anio = album.Anio,
            PortadaUrl = album.PortadaUrl,
            Canciones = _canciones.GetAll().Where(x => x.IdAlbum == id).Select(x => new CancionDto
            {
                IdCancion = x.IdCancion,
                Nombre = x.Nombre,
                IdArtista = x.IdArtista,
                IdAlbum = x.IdAlbum,
                Artista = artista?.Nombre ?? "N/D",
                Album = album.Titulo,
                Duracion = x.Duracion,
                AudioUrl = x.AudioUrl,
                ImagenUrl = x.ImagenUrl
            }).ToList()
        };
    }
}
