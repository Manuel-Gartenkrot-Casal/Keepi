using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
public class HeladeraController : Controller
{
    private Heladera? Heladera;

    public HeladeraController()
    {

    }
    public IActionResult InicializarHeladera(){
        Heladera = BD.GetHeladeraByUsuarioId(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        HttpContext.Session.SetString("nombreHeladera", Heladera.Nombre);
        return RedirectToAction("CargarProductos");
    }
    public IActionResult CambiarColor(){
        // Obtener la heladera desde la sesión cuando se necesite
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        return View();
    }
    
    [HttpPost]
    public IActionResult CambiarColor(string color, string nombreHeladera){
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        if (Heladera != null)
        {
            Heladera.CambiarColor(color);
        }
        return View();
    }

    public IActionResult EliminarHeladera(string nombreHeladera, string username) {

        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));

        int resultado = Heladera.EliminarHeladera(username);

        if (resultado == -1) {
            
            Console.WriteLine("No se pudo borrar");
            ViewBag.mensajeEliminar = "No se pudo borrar";
        }
        else {
            Console.WriteLine("Se borró correctamente");
            ViewBag.mensajeEliminar = "Se borró correctamente";
        }

    return View("MiHeladera");
    }
    public IActionResult CargarProductos(){
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        ViewBag.Productos = BD.GetProductosByHeladeraId(Heladera.ID);
        return View("MiHeladera");
    }
}