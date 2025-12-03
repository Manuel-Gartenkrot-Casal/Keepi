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

                //Se crea una heladera automática SIN requerir 'nombre'
                Console.WriteLine("No tenías heladeras creadas. Se te creó una heladera automática. Puedes volver a tu cuenta");

                try
                {
                    // Crear heladera automática
                    Heladera heladeraNueva = new Heladera();
                    int resultado = heladeraNueva.AgregarHeladera("Mi Primera Heladera", "#ffffff");

                    if (resultado == -1)
                    {
                        Console.WriteLine("No se pudo crear heladera automática");
                        return Json(new { success = false, message = "No se pudo crear la heladera automática." });
                    }

                    // Obtener ID
                    int idHeladera = BD.GetIdHeladeraByNombre("Mi Primera Heladera");
                    if (idHeladera <= 0)
                        return Json(new { success = false, message = "Error al obtener ID de la heladera automática" });

                    // Asociar al usuario
                    BD.AsociarHeladeraAUsuario(usuario.ID, idHeladera);

                    Console.WriteLine("Heladera automática creada correctamente");

                    return Json(new { success = true, message = "Heladera automática creada correctamente" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en AgregarHeladera: {ex.Message}");
                    return Json(new { success = false, message = "Error al crear la heladera automática: " + ex.Message });
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
        lista = BD.BuscarHeladeraDeUsuario(idUsuario);
        ViewBag.Heladeras = lista;
        return View("Heladeras");
    }

    [HttpPost]
    public IActionResult CambiarColor(string color, string nombreHeladera)
    {
        Console.WriteLine("Entré a cambiar color");
        int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        if (idUsuario == null)
        {
            Console.WriteLine("Usuario no autenticado");
            return RedirectToAction("Login", "Auth");
        }
        Heladera Heladera = BD.SeleccionarHeladeraByNombre(idUsuario.Value, nombreHeladera);
        if (Heladera != null)
        {
            Heladera.CambiarColor(color, Heladera.ID);
        }
        else {
            Console.WriteLine("No se encontró la heladera para cambiar color");
        }
        return RedirectToAction("Heladeras");
    }

    [HttpPost]
    public IActionResult CambiarNombre(string nuevoNombre, string nombreHeladera)
    {
        Console.WriteLine("Entré a cambiar nombre");
        int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        if (idUsuario == null)
        {
            Console.WriteLine("Usuario no autenticado");
            return RedirectToAction("Login", "Auth");
        }

        Heladera Heladera = BD.SeleccionarHeladeraByNombre(idUsuario.Value, nombreHeladera);
        if (Heladera != null)
        {
            Heladera.CambiarNombre(nuevoNombre, Heladera.ID);
        }
        else {
            Console.WriteLine("No se encontró la heladera para cambiar nombre");
        }

        return RedirectToAction("Heladeras");

    }

    [HttpPost]
    public IActionResult EliminarHeladera(string nombreHeladera)
    {
        try
        {
            string user = HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            Usuario usuario = Objeto.StringToObject<Usuario>(user);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error al obtener usuario" });
            }

            if (string.IsNullOrEmpty(nombreHeladera))
            {
                return Json(new { success = false, message = "Nombre de heladera no especificado" });
            }

            Heladera heladera = BD.SeleccionarHeladeraByNombre(usuario.ID, nombreHeladera);
            if (heladera == null)
            {
                return Json(new { success = false, message = "Heladera no encontrada" });
            }

            int resultado = heladera.EliminarHeladera(usuario.Username);

            if (resultado == -1)
            {
                Console.WriteLine("No se pudo borrar heladera: " + nombreHeladera);
                return Json(new { success = false, message = "No se pudo eliminar la heladera" });
            }
            else
            {
                Console.WriteLine("Se borró correctamente: " + nombreHeladera);
                return Json(new { success = true, message = "Heladera eliminada correctamente" });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en EliminarHeladera: {ex.Message}");
            return Json(new { success = false, message = "Error al eliminar la heladera: " + ex.Message });
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

    [HttpPost]
    public IActionResult AgregarHeladera(string nombre, string color)
    {
        try
        {
            string user = HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            Usuario usuario = Objeto.StringToObject<Usuario>(user);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error al obtener usuario" });
            }

            if (string.IsNullOrEmpty(nombre))
            {
                return Json(new { success = false, message = "El nombre de la heladera es requerido" });
            }

            Heladera heladera = new Heladera();
            int resultado = heladera.AgregarHeladera(nombre, color);

            if (resultado == -1)
            {
                Console.WriteLine($"No se pudo crear heladera: {nombre}");
                return Json(new { success = false, message = "No se pudo crear la heladera. Es posible que ya exista una con ese nombre." });
            }
            else
            {
                // Asociar la heladera al usuario
                int idHeladera = BD.GetIdHeladeraByNombre(nombre);
                if (idHeladera > 0)
                {
                    BD.AsociarHeladeraAUsuario(usuario.ID, idHeladera);
                    Console.WriteLine($"Heladera creada correctamente: {nombre}");
                    return Json(new { success = true, message = "Heladera creada correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Error al asociar la heladera al usuario" });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en AgregarHeladera: {ex.Message}");
            return Json(new { success = false, message = "Error al crear la heladera: " + ex.Message });
        }
    }
}


