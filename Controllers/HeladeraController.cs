using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
                Console.WriteLine($"[HeladeraController] Usuario {idUsuario} no tiene heladeras, creando una por defecto");
                
                int resultado = BD.agregarHeladera($"Mi Heladera", "#7ecb20", idUsuario);
                if (resultado > 0)
                {
                    nombresHeladeras = BD.traerNombresHeladerasById(idUsuario);
                }
                else
                {
                    TempData["Error"] = "Error al crear heladera por defecto.";
                    return RedirectToAction("Index", "Home");
                }
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
        lista = BD.BuscarHeladeraDeUsuario(user);
        ViewBag.Heladeras = lista;
        return View();
    }

    public IActionResult CambiarColor()
    {
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        return View();
    }

    [HttpPost]
    public IActionResult CambiarColor(string color)
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return Json(new { success = false, message = "No autorizado" });
        }
        
        Usuario usuario = Objeto.StringToObject<Usuario>(user);
        string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");
        Heladera heladera = BD.SeleccionarHeladeraByNombre(usuario.ID, nombreHeladera);
        
        if (heladera != null)
        {
            heladera.CambiarColor(color);
            // Aquí deberías actualizar en la base de datos
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Heladera no encontrada" });
    }

[HttpPost]
public IActionResult EliminarHeladera(string nombreHeladera)
{
    try
    {
        // Get the username from session
        var username = HttpContext.Session.GetString("usuario");
        if (string.IsNullOrEmpty(username))
        {
            return Json(new { success = false, message = "Usuario no autenticado" });
        }

        // Get the user ID from session
        var idUsuarioStr = HttpContext.Session.GetString("IdUsuario");
        if (string.IsNullOrEmpty(idUsuarioStr) || !int.TryParse(idUsuarioStr, out int idUsuario))
        {
            return Json(new { success = false, message = "ID de usuario inválido" });
        }

        // Get the Heladera object
        var heladera = BD.SeleccionarHeladeraByNombre(idUsuario, nombreHeladera);
        if (heladera == null)
        {
            return Json(new { success = false, message = "Heladera no encontrada" });
        }

        // Delete the heladera - Update this line
        int resultado = BD.borrarHeladera(heladera.ID, idUsuario);  // Using borrarHeladera with correct parameters

        if (resultado > 0)
        {
            // Clear the session if the current heladera is the one being deleted
            var currentHeladera = HttpContext.Session.GetString("nombreHeladera");
            if (!string.IsNullOrEmpty(currentHeladera) && currentHeladera == nombreHeladera)
            {
                HttpContext.Session.Remove("nombreHeladera");
            }
            return Json(new { success = true, message = "Heladera eliminada correctamente" });
        }
        return Json(new { success = false, message = "No se pudo eliminar la heladera" });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] Error al eliminar heladera: {ex.Message}");
        return Json(new { success = false, message = "Error al procesar la solicitud" });
    }
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

    [HttpPost]
    public IActionResult CambiarTipoAlmacenamiento([FromForm] int idProductoXHeladera, [FromForm] string tipoAlmacenamiento)
    {
        string user = HttpContext.Session.GetString("usuario");
        if (user == null)
        {
            return Json(new { success = false, message = "No autorizado" });
        }
        
        // Validar que el tipo sea válido
        if (tipoAlmacenamiento != "Refrigerado" && tipoAlmacenamiento != "Congelado" && tipoAlmacenamiento != "Alacena")
        {
            return Json(new { success = false, message = "Tipo de almacenamiento inválido" });
        }
        
        BD.ActualizarTipoAlmacenamientoProducto(idProductoXHeladera, tipoAlmacenamiento);
        return Json(new { success = true });
    }

    public IActionResult TraerNombresHeladera()
    {
        ViewBag.NombresHeladeras = BD.traerNombresHeladerasById(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        return View("MiHeladera");

    }

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

    [HttpPost]
    public IActionResult CrearHeladera(string nombreHeladera, string colorHeladera)
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
            
            int resultado = BD.agregarHeladera(nombreHeladera, colorHeladera ?? "#7ecb20", usuario.ID);
            if (resultado > 0)
            {
                return Json(new { success = true, message = "Heladera creada exitosamente" });
            }
            else
            {
                return Json(new { success = false, message = "Error al crear heladera" });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HeladeraController] Error en CrearHeladera: {ex.Message}");
            return Json(new { success = false, message = "Error al crear heladera" });
        }
    }
}
