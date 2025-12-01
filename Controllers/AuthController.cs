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
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            TempData["Error"] = "Usuario y contraseña son requeridos";
            return RedirectToAction("Login");
        }

        try
        {
            Usuario user = BD.verificarUsuario(Username, Password);
            if (user == null)
            {
                Console.WriteLine($"[AuthController] Login fallido para usuario: {Username}");
                TempData["Error"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("Login");
            }
            
            Console.WriteLine($"[AuthController] Login exitoso - Usuario ID: {user.ID}, Username: {user.Username}");
            string usuario = Objeto.ObjectToString(user);
            HttpContext.Session.SetString("usuario", usuario);
            HttpContext.Session.SetInt32("IdUsuario", user.ID);
            
            return RedirectToAction("InicializarHeladera", "Heladera");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthController] Error en verificarCuenta: {ex.Message}");
            TempData["Error"] = "Error al iniciar sesión. Intente nuevamente.";
            return RedirectToAction("Login");
        }
    }
    
   [HttpPost]
public IActionResult Registrarse(string Username, string Password, string Name, string Ape, string Email)
{
    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
    {
        TempData["ErrorRegistro"] = "Todos los campos son obligatorios"; // Usar ErrorRegistro para que el script reabra el form
        return RedirectToAction("Login");
    }

    try
    {
        // Llamamos al método actualizado pasando Name como nombre y Ape como apellido
        int sePudo = BD.crearUsuario(Username, Password, Name, Ape, Email);
        
        if (sePudo == 1)
        {
            Usuario user = BD.verificarUsuario(Username, Password);
            if (user != null)
            {
                Console.WriteLine($"[AuthController] Registro exitoso - Usuario ID: {user.ID}");
                string usuarioJson = Objeto.ObjectToString(user);
                HttpContext.Session.SetString("usuario", usuarioJson);
                HttpContext.Session.SetInt32("IdUsuario", user.ID);
                return RedirectToAction("InicializarHeladera", "Heladera");
            }
        }
        
        TempData["ErrorRegistro"] = "El usuario ya existe o hubo un error.";
        return RedirectToAction("Login");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[AuthController] Error en Registrarse: {ex.Message}");
        TempData["ErrorRegistro"] = "Error interno al registrar usuario.";
        return RedirectToAction("Login");
    }
}
}