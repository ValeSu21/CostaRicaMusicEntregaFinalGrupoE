using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using CostaRicaMusicDAL.Repositories;
namespace CostaRicaMusicBLL.Services;
public class CancionService : ICancionService
{
    private readonly ICancionRepository _canciones;
    private readonly IArtistaRepository _artistas;
    private readonly IAlbumRepository _albumes;
    public CancionService(ICancionRepository canciones, IArtistaRepository artistas, IAlbumRepository albumes)
    {
        _canciones = canciones;
        _artistas = artistas;
        _albumes = albumes;
    }
    public List<CancionDto> GetAll() => _canciones.GetAll().Select(Map).ToList();
    public CancionDto? GetById(int id)
    {
        var item = _canciones.GetById(id);
        return item is null ? null : Map(item);
    }
    public PagedResultDto<CancionDto> GetPaged(string? search, int page, int pageSize)
    {
        var query = _canciones.GetAll().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
        var total = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize).Select(Map).ToList();
        return new PagedResultDto<CancionDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            Search = search ?? string.Empty
        };
    }
    private CancionDto Map(CostaRicaMusicDAL.Entidades.Cancion x)
    {
        var artista = _artistas.GetById(x.IdArtista);
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
    }
}
