using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeepiProg.Models;

namespace KeepiProg.Controllers;

public class NotificacionesController : Controller {

    private readonly ILogger<NotificacionesController> _logger;

    private static List<Notificacion> Notis = new List<Notificacion>();

    public NotificacionesController(ILogger<NotificacionesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Panel() 
    {
        ViewBag.Notificaciones = Notis;
        return PartialView("_Notificacion");
    }

    /* public IActionResult CrearNotificacion(string nombreProducto) { //el nombreProducto siendo NombreEsp
        
        ProductoXHeladera product = Objeto.StringToObject<ProductoXHeladera>(HttpContext.Session.GetString("ID")); //Guarden los productos con ID porque son y todos distintos (porque tienen distinta heladera por ejemplo)
        
        int DiasRestantes = product.ObtenerDiasRestantes();
        
        int PorcentajeRestante = product.CalcularPorcentajeRestante();
            
        string mensaje = "";

        if ((PorcentajeRestante == 50 || PorcentajeRestante == 25 || PorcentajeRestante == 10) && DiasRestantes != 1) { //mensajeDeCuandoFaltaLaMitad
            mensaje = $"Faltan {DiasRestantes} días para el vencimiento de su {nombreProducto}. Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }
        else if (DiasRestantes == 1) {
            mensaje = $"Falta {DiasRestantes} día para el vencimiento de su {nombreProducto}. Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }


        if (!string.IsNullOrEmpty(mensajes)) 
        {  
            ViewBag.mensaje = mensaje;
            Notificacion.CrearNoti(mensaje);
        }

        string returnUrl = Request.Headers["Referer"].ToString(); //Ésto sirve para volver a página anterior
        return RedirectToAction(returnUrl);
    }*/
}
