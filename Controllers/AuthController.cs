using Microsoft.AspNetCore.Mvc;
namespace KEEPI;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult verificarCuenta(string Username, string Password)
    {
        Usuario user = BD.verificarUsuario(Username, Password);
        if (user != null)
        {
            HttpContext.Session.SetString("usuario", Objeto.ObjectToString(user));
            return RedirectToAction("Logged", "Home");
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
            Usuario user = BD.verificarUsuario(Username, Password);
            HttpContext.Session.SetString("usuario", Objeto.ObjectToString(user));
            return RedirectToAction("Logged", "Home");
        }
        else{
             ViewBag.Error = "Usuario o contraseña ya existentes";
            return View("Index");

        }
    }

}