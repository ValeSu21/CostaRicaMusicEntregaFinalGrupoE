using System.Security.Claims;
using CostaRicaMusicBLL.DTOs;
using CostaRicaMusicBLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CostaRicaMusic.Controllers;

public class LoginController : Controller
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService) => _authService = authService;

    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Canciones");
        }

        return View(new LoginDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var user = _authService.ValidateUser(dto);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            return View(dto);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Nombre),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Rol)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        TempData["SuccessMessage"] = $"Bienvenido, {user.Nombre}.";
        return RedirectToAction("Index", "Canciones");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult AccessDenied() => Content("Acceso denegado");
}
