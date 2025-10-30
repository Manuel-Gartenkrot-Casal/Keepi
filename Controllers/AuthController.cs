using Microsoft.AspNetCore.Mvc;
namespace KEEPI;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }
    public IActionResult Login()
    {
        
        return View();
    }
    public BD BD = new BD();
    [HttpPost]
    public IActionResult verificarCuenta(string Username, string Password)
    {
        string user = BD.verificarUsuario(Username, Password);
        if (user != "Error")
        {
            HttpContext.Session.SetString("usuario", user);
            return RedirectToAction("InicializarHeladera", "Heladera");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpPost]
    public IActionResult Registrarse(string Username, string Password)
    {
        int sePudo = BD.crearUsuario(Username, Password);
        if (sePudo == 1)
        {
            string user = BD.verificarUsuario(Username, Password);
            HttpContext.Session.SetString("usuario", user);
            return RedirectToAction("InicializarHeladera", "Heladera");
        }
        else{
             ViewBag.Error = "Usuario o contraseña ya existentes";
            return View("Index");

        }
    }

}