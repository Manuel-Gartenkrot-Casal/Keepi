using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeepiProg.Models;

namespace KeepiProg.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
     private int? GetCurrentUserId()


        
        {
          return HttpContext.Session.GetInt32("IdUsuario");
        }
 public IActionResult Perfil()
{
    int? idUsuario = GetCurrentUserId();
    if (idUsuario == null)
    {
        return RedirectToAction("Login", "Auth");
    }
    string usuarioJson = HttpContext.Session.GetString("usuario");
    ViewBag.UsuarioJson = usuarioJson;
    return View();
}


}
