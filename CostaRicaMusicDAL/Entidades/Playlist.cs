namespace CostaRicaMusicDAL.Entidades;
public class Playlist
{
    public int IdPlaylist { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string UsuarioEmail { get; set; } = string.Empty;
    public List<int> CancionesIds { get; set; } = new();
}
