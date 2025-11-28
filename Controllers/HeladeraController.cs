using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using Keepi.Models;
public class HeladeraController : Controller
{
    private Heladera? Heladera;

    public HeladeraController()
    {

    }
    public BD BD = new BD();
    public IActionResult InicializarHeladera()
    {
        try
        {
            string user = HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                Console.WriteLine("[HeladeraController] Usuario no autenticado en InicializarHeladera");
                TempData["Error"] = "Sesión expirada. Por favor, inicia sesión.";
                return RedirectToAction("Login", "Auth");
            }
            
            Usuario usuario = Objeto.StringToObject<Usuario>(user);
            if (usuario == null)
            {
                Console.WriteLine("[HeladeraController] Error al deserializar usuario");
                return RedirectToAction("Login", "Auth");
            }
            
            int idUsuario = usuario.ID;
            Console.WriteLine($"[HeladeraController] Inicializando heladera para usuario ID: {idUsuario}");

            List<string> nombresHeladeras = BD.traerNombresHeladerasById(idUsuario);
            if (nombresHeladeras == null || nombresHeladeras.Count == 0)
            {
                Console.WriteLine($"[HeladeraController] Usuario {idUsuario} no tiene heladeras");
                TempData["Error"] = "No tienes heladeras creadas. Por favor, crea una heladera primero.";
                // TODO: Redirigir a una página para crear heladera
                return RedirectToAction("Index", "Home");
            }
            
            // Establecer la primera heladera como la heladera actual
            string nombrePrimeraHeladera = nombresHeladeras[0];
            Console.WriteLine($"[HeladeraController] Seleccionando heladera: {nombrePrimeraHeladera}");
            
            Heladera heladera = BD.SeleccionarHeladeraByNombre(idUsuario, nombrePrimeraHeladera);
            if (heladera == null)
            {
                Console.WriteLine($"[HeladeraController] Error: No se pudo obtener la heladera {nombrePrimeraHeladera}");
                TempData["Error"] = "Error al cargar la heladera.";
                return RedirectToAction("Index", "Home");
            }
            
            HttpContext.Session.SetString("nombreHeladera", heladera.Nombre);
            Console.WriteLine($"[HeladeraController] Heladera '{heladera.Nombre}' establecida en sesión para usuario {idUsuario}");
            
            return RedirectToAction("CargarProductos");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HeladeraController] Error en InicializarHeladera: {ex.Message}");
            Console.WriteLine($"[HeladeraController] StackTrace: {ex.StackTrace}");
            TempData["Error"] = "Error al inicializar la heladera.";
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Heladeras() {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        Usuario usuario = Objeto.StringToObject<Usuario>(user);
        int idUsuario = usuario.ID;
        List<string> nombresHeladeras = BD.traerNombresHeladerasById(idUsuario);
        if (nombresHeladeras == null || nombresHeladeras.Count == 0)
        {
            return RedirectToAction("CrearHeladera", "Heladera"); //Si no hay heladera, se crea una o te vas
        }
        List<Heladera> lista = new List<Heladera>();
        ViewBag.Heladeras = lista;
        return View("Heladeras");
    }


    public IActionResult CambiarColor()
    {
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        return View();
    }

    [HttpPost]
    public IActionResult CambiarColor(string color, string nombreHeladera)
    {
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        if (Heladera != null)
        {
            Heladera.CambiarColor(color);
        }
        return View();
    }

    public IActionResult EliminarHeladera(string nombreHeladera, string username)
    {

        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));

        int resultado = Heladera.EliminarHeladera(username);

        if (resultado == -1)
        {

            Console.WriteLine("No se pudo borrar");
            ViewBag.mensajeEliminar = "No se pudo borrar";
        }
        else
        {
            Console.WriteLine("Se borró correctamente");
            ViewBag.mensajeEliminar = "Se borró correctamente";
        }

        return View("MiHeladera");
    }

public IActionResult CargarProductos()
{
    string user = HttpContext.Session.GetString("usuario");
    if (user == null)
    {
        return RedirectToAction("Login", "Auth");
    }
    Usuario usuario = Objeto.StringToObject<Usuario>(user);
    string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");
    if (string.IsNullOrEmpty(nombreHeladera))
    {
        return RedirectToAction("InicializarHeladera");
    }
    Heladera Heladera = BD.SeleccionarHeladeraByNombre(usuario.ID, nombreHeladera);
    
    if (Heladera != null)
    {
        ViewBag.Productos = BD.GetProductosXHeladeraByHeladeraId(Heladera.ID);
    }
    else
    {
        ViewBag.Productos = new List<ProductoXHeladera>();
    }

    try {
        ViewBag.ListaProductos = BD.GetAllProductos(); 
    } catch {
        ViewBag.ListaProductos = new List<Producto>();
    }

    ViewBag.ColorHeladera = Heladera.Color;
    return View("MiHeladera");
}

    [HttpPost]
    public IActionResult EliminarProducto([FromForm] int idProductoXHeladera, [FromForm] int idProducto)
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return Json(new { success = false, message = "No autorizado" });
        }
        Usuario usuario = Objeto.StringToObject<Usuario>(user);
        string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");
        Heladera Heladera = BD.SeleccionarHeladeraByNombre(usuario.ID, nombreHeladera);
        
        if (Heladera != null)
        {
            BD.EliminarProductoXHeladera(Heladera.ID, idProducto);
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Heladera no encontrada" });
    }

    [HttpPost]
    public IActionResult CambiarEstadoAbierto([FromForm] int idProductoXHeladera, [FromForm] bool abierto)
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return Json(new { success = false, message = "No autorizado" });
        }
        BD.ActualizarAbiertoProducto(idProductoXHeladera, abierto);
        return Json(new { success = true });
    }

    public IActionResult TraerNombresHeladera()
    {
        ViewBag.NombresHeladeras = BD.traerNombresHeladerasById(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        return View("MiHeladera");

    }



/*
    public IActionResult Heladeras()
    {
        try
        {
            string user = HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                Console.WriteLine("[HeladeraController] Usuario no autenticado en Heladeras");
                TempData["Error"] = "Sesión expirada. Por favor, inicia sesión.";
                return RedirectToAction("Login", "Auth");
            }
            
            Usuario usuario = Objeto.StringToObject<Usuario>(user);
            if (usuario == null)
            {
                Console.WriteLine("[HeladeraController] Error al deserializar usuario");
                return RedirectToAction("Login", "Auth");
            }
            
            int idUsuario = usuario.ID;
            Console.WriteLine($"[HeladeraController] Cargando heladeras para usuario ID: {idUsuario}");

            List<string> nombresHeladeras = BD.traerNombresHeladerasById(idUsuario);
            if (nombresHeladeras == null || nombresHeladeras.Count == 0)
            {
                Console.WriteLine($"[HeladeraController] Usuario {idUsuario} no tiene heladeras");
                TempData["Error"] = "No tienes heladeras creadas. Por favor, crea una heladera primero.";
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.NombresHeladeras = nombresHeladeras;
            ViewBag.HeladeraActual = HttpContext.Session.GetString("nombreHeladera");
            
            return View("Heladeras");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HeladeraController] Error en Heladeras: {ex.Message}");
            TempData["Error"] = "Error al cargar las heladeras.";
            return RedirectToAction("Index", "Home");
        }
    }
*/

    [HttpPost]
    public IActionResult CambiarHeladera(string nombreHeladera)
    {
        try
        {
            string user = HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                return Json(new { success = false, message = "No autorizado" });
            }
            
            Usuario usuario = Objeto.StringToObject<Usuario>(user);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de sesión" });
            }
            
            // Verificar que la heladera pertenece al usuario
            Heladera heladera = BD.SeleccionarHeladeraByNombre(usuario.ID, nombreHeladera);
            if (heladera == null)
            {
                return Json(new { success = false, message = "Heladera no encontrada" });
            }
            
            HttpContext.Session.SetString("nombreHeladera", heladera.Nombre);
            Console.WriteLine($"[HeladeraController] Heladera cambiada a '{heladera.Nombre}' para usuario {usuario.ID}");
            
            return Json(new { success = true, message = "Heladera cambiada exitosamente" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HeladeraController] Error en CambiarHeladera: {ex.Message}");
            return Json(new { success = false, message = "Error al cambiar heladera" });
        }
    }
}
