using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeepiProg.Models;

namespace KeepiProg.Controllers;

public class NotificacionesController : Controller {

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public NotificacionesController() {

    }

    public IActionResult CrearNotificacion(string nombreProducto) {
        productoXHeladera product = Objeto.StringToObject<ProductoXHeladera>(HttpContext.Session.GetString("ID")); //Guarden los productos con ID porque son y todos distintos (porque tienen distinta heladera por ejemplo)
        DiasRestantes = product.ObtenerDiasRestantes();
        PorcentajeRestante = product.CalcularPorcentajeRestante();
        nombreProducto = product.NombreEsp;
        if (PorcentajeRestante == 50 && DiasRestantes != 1) { //mensajeDeCuandoFaltaLaMitad
            string mensaje = "Faltan " + DiasRestantes + " días para el vencimiento de su " + nombreProducto + ". Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }
        else if (PorcentajeRestante == 25 && DiasRestantes != 1) { //mensajeDeCuandoFaltaUnCuarto
            string mensaje = "Faltan " + DiasRestantes + " días para el vencimiento de su " + nombreProducto + ". Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }
        else if (PorcentajeRestante == 10 && DiasRestantes != 1) {
            string mensaje = "Faltan " + DiasRestantes + " días para el vencimiento de su " + nombreProducto + ". Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }
        else if (DiasRestantes == 1) {
            string mensaje = "Falta " + DiasRestantes + " día para el vencimiento de su " + nombreProducto + ". Presione aquí abajo si quiere ver recetas para hacer con ésto:";
        }
        ViewBag.mensaje = mensaje;
    }
}