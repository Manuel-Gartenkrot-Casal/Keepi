using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
public class HeladeraController : Controller
{
    private Heladera? Heladera;

    public HeladeraController()
    {
        // Inicializar en el constructor si es necesario
    }

    public IActionResult CambiarColor(){
        // Obtener la heladera desde la sesión cuando se necesite
        Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("Heladera"));
        return View();
    }
    
    [HttpPost]
    public IActionResult CambiarColor(string color){
        Heladera = Objeto.StringToObject<Heladera>(HttpContext.Session.GetString("Heladera"));
        if (Heladera != null)
        {
            Heladera.CambiarColor(color);
        }
        return View();
    }
}