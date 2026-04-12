using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CostaRicaMusic.Controllers;
[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Canciones");
}
