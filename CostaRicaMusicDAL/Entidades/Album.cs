namespace CostaRicaMusicDAL.Entidades;
public class Album
{
    public int IdAlbum { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int IdArtista { get; set; }
    public int Anio { get; set; }
    public string PortadaUrl { get; set; } = string.Empty;
}
