using System.Security.Claims;
using CostaRicaMusicBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostaRicaMusic.Controllers;

[Authorize]
public class CancionesController : Controller
{
    private readonly ICancionService _service;
    private readonly IPlaylistService _playlists;

    public CancionesController(ICancionService service, IPlaylistService playlists)
    {
        _service = service;
        _playlists = playlists;
    }

    public IActionResult Index(string? search, int page = 1)
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        ViewBag.Playlists = _playlists.GetByUser(email);
        var result = _service.GetPaged(search, page, 4);
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AgregarAPlaylist(int playlistId, int songId)
    {
        _playlists.AddSong(playlistId, songId);
        TempData["SuccessMessage"] = "La canción fue agregada a la playlist.";
        return RedirectToAction(nameof(Index));
    }
}
