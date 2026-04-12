namespace CostaRicaMusicBLL.DTOs;
public class AlbumDto
{
    public int IdAlbum { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int IdArtista { get; set; }
    public string Artista { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string PortadaUrl { get; set; } = string.Empty;
    public List<CancionDto> Canciones { get; set; } = new();
}
