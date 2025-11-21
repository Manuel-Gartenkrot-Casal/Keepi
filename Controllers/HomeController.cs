using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeepiProg.Models;
using Keepi.Models;

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
    
    public IActionResult AgregarProducto()
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        
        ViewBag.Productos = BD.GetAllProductos();
        return View();
    }
    
    [HttpPost]
    public IActionResult AgregarProducto(int idProducto, string nombreEspecifico, DateTime fechaVencimiento, string foto)
    {
        try
        {
            string user = HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                return Json(new { success = false, message = "No autorizado" });
            }
            
            Usuario usuario = Objeto.StringToObject<Usuario>(user);
            string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");
            
            if (string.IsNullOrEmpty(nombreHeladera))
            {
                return Json(new { success = false, message = "No hay heladera seleccionada" });
            }
            
            Heladera heladera = BD.SeleccionarHeladeraByNombre(usuario.ID, nombreHeladera);
            
            if (heladera == null)
            {
                return Json(new { success = false, message = "Heladera no encontrada" });
            }
            
            BD.agregarProductoExistente(heladera.ID, idProducto, nombreEspecifico, fechaVencimiento, foto);
            
            return Json(new { success = true, message = "Producto agregado exitosamente" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HomeController] Error en AgregarProducto: {ex.Message}");
            return Json(new { success = false, message = "Error al agregar producto" });
        }
    }
    
    public IActionResult ListaDeSuper()
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        
        return View();
    }
    
    public IActionResult Perfil()
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        
        Usuario usuario = Objeto.StringToObject<Usuario>(user);
        return View(usuario);
    }
}
