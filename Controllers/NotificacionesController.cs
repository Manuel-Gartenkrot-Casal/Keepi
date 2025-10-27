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
        if (PorcentajeRestante <= 5) {
            
        }
    }
}