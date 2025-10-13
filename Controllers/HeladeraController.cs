using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
public class HeladeraController : Controller
{
    Heladera Heladera = Objeto.StringToObject(HttpContext.Session.GetString("Heladera"));

    public IActionResult CambiarColor(){
        return View();
    }
    [HttpPost]
    public IActionResult CambiarColor(string color){
        Heladera.CambiarColor(color);
        return View();
    }
}