using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeepiProg.Models;
using Keepi.Models;

namespace KeepiProg.Controllers;

public class NotificacionesController : Controller {

    private readonly ILogger<NotificacionesController> _logger;

    private static List<Notificacion> Notis = new List<Notificacion>();

    public NotificacionesController(ILogger<NotificacionesController> logger)
    {
        _logger = logger;
    }

    public IActionResult CrearNotificacion(int idProductoXHeladera) { 
        
        ProductoXHeladera pXh = BD.TraerProductoXHeladeraByID(idProductoXHeladera);
        
        int DiasRestantes = pXh.ObtenerDiasRestantes();
        
        double PorcentajeRestante = pXh.CalcularPorcentajeRestante();

        int porcentajeInt = (int)Math.Round(PorcentajeRestante); //Redondear para evitar decimales
            
        string mensaje = "";

        int resultado = -1;

        if ((porcentajeInt == 50 || porcentajeInt == 25 || porcentajeInt == 10) && DiasRestantes != 1) { //mensajeDeCuandoFaltaLaMitad
            mensaje = $"Faltan {DiasRestantes} días para el vencimiento de su {pXh.NombreEspecifico}. Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }
        else if (DiasRestantes == 1) {
            mensaje = $"Falta {DiasRestantes} día para el vencimiento de su {pXh.NombreEspecifico}. Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }

        if (!string.IsNullOrEmpty(mensaje)) 
        {  
            ViewBag.mensaje = mensaje;
            DateTime hoy = DateTime.Now.Date;
            Notificacion noti = new Notificacion();
            resultado = noti.CrearNotificacion(mensaje, hoy, idProductoXHeladera);
        }

        if (resultado == -1) {
            Console.WriteLine("No se pudo crear la notificacion");
        }

        string returnUrl = Request.Headers["Referer"].ToString(); //Ésto sirve para volver a página anterior
        return Redirect(returnUrl);
    }

    public IActionResult BorrarNotificacion(int ID) {

        Notificacion noti = BD.traerNotificacionById(ID);

        int resultado = -1;

        if (noti != null) {
            resultado = noti.BorrarNotificacion();
        }
        else {
            Console.WriteLine("No se encontro la notificacion");
        }

        if (resultado == -1) {
            Console.WriteLine("No se pudo borrar la notificacion");
        }

        string returnUrl = Request.Headers["Referer"].ToString(); //Ésto sirve para volver a página anterior
        return Redirect(returnUrl);
    }

    public IActionResult MarcarNotificacionComoLeida(int ID) {

        Notificacion noti = BD.traerNotificacionById(ID);

        int resultado = -1;

        if (noti != null) {
            resultado = noti.NotiLeida();
        }
        else {
            Console.WriteLine("No se encontro la notificacion");
        }

        if (resultado == -1) {
            Console.WriteLine("No se pudo marcar la notificacion como leída");
        }

        string returnUrl = Request.Headers["Referer"].ToString(); //Ésto sirve para volver a página anterior
        return Redirect(returnUrl);
    }
}


