using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        Heladera Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("nombreHeladera"));
        ViewBag.Productos = BD.GetProductosByHeladeraId(Heladera.ID);
        return View("MiHeladera");
    }
    public IActionResult TraerNombresHeladera()
    {
        ViewBag.NombresHeladeras = BD.traerNombresHeladerasById(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        return View("MiHeladera");

    }

    /*public IActionResult CambiarHeladera(string NombreHeladera){
        
    }*/
}