using Microsoft.AspNetCore.Mvc;        // ← ViewComponent, IViewComponentResult
using Keepi.Services;                  // ← INotificacionService
using System;                          // Opcional, pero común

public class NotificacionesViewComponent : ViewComponent
{
    private readonly INotificacionService _service;

    public NotificacionesViewComponent(INotificacionService service)
    {
        _service = service;
    }

    public IViewComponentResult Invoke()
    {
        int? usuarioId = HttpContext.Session.GetInt32("IdUsuario");

        if (usuarioId == null || usuarioId < 0) {
            Console.WriteLine("Sesión no iniciada");
            return Content(""); // Despues lo arreglo, tiene que mandar una advertencia
        }  
        else {
            var notificaciones = _service.ObtenerNotificacionesDelUsuario(usuarioId.Value);
            return View("Default", notificaciones);
        }
    }
}
