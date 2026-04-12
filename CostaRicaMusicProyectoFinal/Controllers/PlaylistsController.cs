using System.Security.Claims;
using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostaRicaMusic.Controllers;

[Authorize]
public class PlaylistsController : Controller
{
    private readonly IPlaylistService _service;

    public PlaylistsController(IPlaylistService service) => _service = service;

    public IActionResult Index()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var data = _service.GetByUser(email);
        return View(data);
    }

    [HttpGet]
    public IActionResult Create() => View(new PlaylistDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PlaylistDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        dto.UsuarioEmail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        _service.Create(dto);
        TempData["SuccessMessage"] = "Playlist creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var dto = _service.GetById(id);
        return dto is null ? NotFound() : View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(PlaylistDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        dto.UsuarioEmail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        _service.Update(dto);
        TempData["SuccessMessage"] = "Playlist actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        TempData["SuccessMessage"] = "Playlist eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveSong(int playlistId, int songId)
    {
        _service.RemoveSong(playlistId, songId);
        TempData["SuccessMessage"] = "La canción fue eliminada de la playlist.";
        return RedirectToAction(nameof(Index));
    }
}
