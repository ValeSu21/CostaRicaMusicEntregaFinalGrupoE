namespace CostaRicaMusicDAL.Entidades;
public class Artista
{
    public int IdArtista { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string Biografia { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
}
