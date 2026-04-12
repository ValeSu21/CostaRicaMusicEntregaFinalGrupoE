using System.ComponentModel.DataAnnotations;
namespace CostaRicaMusicBLL.DTOs;
public class PlaylistDto
{
    public int IdPlaylist { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string UsuarioEmail { get; set; } = string.Empty;
    public List<CancionDto> Canciones { get; set; } = new();
}
