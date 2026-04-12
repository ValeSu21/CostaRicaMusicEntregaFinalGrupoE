using CostaRicaMusicBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CostaRicaMusic.Controllers;
[Authorize]
public class ArtistasController : Controller
{
    private readonly IArtistaService _service;
    public ArtistasController(IArtistaService service) => _service = service;
    public IActionResult Details(int id)
    {
        var item = _service.GetById(id);
        return item is null ? NotFound() : View(item);
    }
}
