using Microsoft.AspNetCore.Mvc;
using Keepi.Models; 
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; 

namespace Keepi.Controllers
{
    public class AgregarProductoController : Controller
    {
        // EL MÉTODO "Formulario" SE PUEDE BORRAR COMPLETO

        [HttpPost]
        public IActionResult Guardar(int idProducto, string nombreEsp, DateTime fechaVencimiento, string foto)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario"); 
            if (idUsuario == null) return RedirectToAction("Login", "Auth"); 
            
            string? nombreHeladera = HttpContext.Session.GetString("nombreHeladera"); 
            if (string.IsNullOrEmpty(nombreHeladera))
            {
                TempData["Error"] = "Tu sesión ha expirado o no has seleccionado una heladera.";
                return RedirectToAction("CargarProductos", "Heladera"); 
            }
            
            Heladera heladeraActual = BD.SeleccionarHeladeraByNombre(idUsuario.Value, nombreHeladera);
            if (heladeraActual == null)
            {
                TempData["Error"] = "Error al cargar la heladera actual.";
                return RedirectToAction("CargarProductos", "Heladera"); 
            }
            
            int idHeladera = heladeraActual.ID; 


            if (idProducto <= 0)
            {
                TempData["Error"] = "Por favor, selecciona un producto válido de la lista.";
                return RedirectToAction("CargarProductos", "Heladera"); // <--- CAMBIO IMPORTANTE
            }

            try
            {
                BD.agregarProductoExistente(
                    idHeladera, 
                    idProducto,
                    nombreEsp,
                    fechaVencimiento,
                    foto
                );
                TempData["Success"] = "¡Producto agregado con éxito!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo agregar el producto: " + ex.Message;
                return RedirectToAction("CargarProductos", "Heladera"); // <--- CAMBIO IMPORTANTE
            }

            return RedirectToAction("CargarProductos","Heladera"); 
        }
    }
}