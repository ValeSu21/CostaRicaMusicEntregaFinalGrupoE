using CostaRicaMusicBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CostaRicaMusic.Controllers;
[Authorize]
public class AlbumesController : Controller
{
    private readonly IAlbumService _service;
    public AlbumesController(IAlbumService service) => _service = service;
    public IActionResult Details(int id)
    {
        var item = _service.GetById(id);
        return item is null ? NotFound() : View(item);
    }
}
