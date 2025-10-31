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
            Console.WriteLine("entro");
            return RedirectToAction("Login", "Auth");
        }
        else {
            return RedirectToAction ("Heladeras");
        }

        Heladera Heladera = BD.SeleccionarHeladeraByNombre(idUsuario, nombresHeladeras[0]);
        HttpContext.Session.SetString("nombreHeladera", Heladera.Nombre);
        return RedirectToAction("CargarProductos");
    
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

    /*public IActionResult CambiarHeladera(string NombreHeladera){
        
    }*/
}