using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using CostaRicaMusicDAL.Entidades;
using CostaRicaMusicDAL.Repositories;
namespace CostaRicaMusicBLL.Services;
public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _playlists;
    private readonly ICancionRepository _canciones;
    private readonly IArtistaRepository _artistas;
    private readonly IAlbumRepository _albumes;
    public PlaylistService(IPlaylistRepository playlists, ICancionRepository canciones, IArtistaRepository artistas, IAlbumRepository albumes)
    {
        _playlists = playlists;
        _canciones = canciones;
        _artistas = artistas;
        _albumes = albumes;
    }
    public List<PlaylistDto> GetByUser(string email) => _playlists.GetByUser(email).Select(Map).ToList();
    public PlaylistDto? GetById(int id)
    {
        var p = _playlists.GetById(id);
        return p is null ? null : Map(p);
    }
    public int Create(PlaylistDto dto)
    {
        var p = _playlists.Create(new Playlist
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            UsuarioEmail = dto.UsuarioEmail,
            CancionesIds = dto.Canciones.Select(x => x.IdCancion).ToList()
        });
        return p.IdPlaylist;
    }
    public void Update(PlaylistDto dto)
    {
        _playlists.Update(new Playlist
        {
            IdPlaylist = dto.IdPlaylist,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            UsuarioEmail = dto.UsuarioEmail,
            CancionesIds = dto.Canciones.Select(x => x.IdCancion).ToList()
        });
    }
    public void Delete(int id) => _playlists.Delete(id);
    public void AddSong(int playlistId, int songId)
    {
        var p = _playlists.GetById(playlistId);
        if (p is null || p.CancionesIds.Contains(songId)) return;
        p.CancionesIds.Add(songId);
        _playlists.Update(p);
    }
    public void RemoveSong(int playlistId, int songId)
    {
        var p = _playlists.GetById(playlistId);
        if (p is null) return;
        p.CancionesIds.Remove(songId);
        _playlists.Update(p);
    }
    private PlaylistDto Map(Playlist p)
    {
        var canciones = p.CancionesIds.Select(id => _canciones.GetById(id)).Where(x => x is not null).Select(x =>
        {
            var artista = _artistas.GetById(x!.IdArtista);
            var album = _albumes.GetById(x.IdAlbum);
            return new CancionDto
            {
                IdCancion = x.IdCancion,
                Nombre = x.Nombre,
                IdArtista = x.IdArtista,
                IdAlbum = x.IdAlbum,
                Artista = artista?.Nombre ?? "N/D",
                Album = album?.Titulo ?? "N/D",
                Duracion = x.Duracion,
                AudioUrl = x.AudioUrl,
                ImagenUrl = x.ImagenUrl
            };
        }).ToList();
        return new PlaylistDto
        {
            IdPlaylist = p.IdPlaylist,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            UsuarioEmail = p.UsuarioEmail,
            Canciones = canciones
        };
    }
}
