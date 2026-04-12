namespace CostaRicaMusicBLL.DTOs;
public class CancionDto
{
    public int IdCancion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Artista { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public int IdArtista { get; set; }
    public int IdAlbum { get; set; }
    public string Duracion { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
}
