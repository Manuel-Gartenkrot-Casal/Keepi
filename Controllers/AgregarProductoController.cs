using Microsoft.AspNetCore.Mvc;
using Keepi.Models; //
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; 

namespace Keepi.Controllers
{
    public class AgregarProductoController : Controller
    {
        [HttpGet]
        public IActionResult Formulario()
        {
            // --- Verificación de Sesión (Comentada para testear) ---
             int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth"); //
            }

            int? idHeladera = HttpContext.Session.GetInt32("IdHeladeraActual");
            if (idHeladera == null)
            {
                TempData["Error"] = "Por favor, selecciona una heladera antes de agregar productos.";
                return RedirectToAction("Seleccionar", "Heladera"); //
            }
            
            // --- Fin Verificación de Sesión ---
            
            try
            {
                List<Producto> productosBase = BD.GetAllProductos(); //
                ViewBag.ListaProductos = productosBase;
            }
            catch (Exception ex)
            {
                ViewBag.ListaProductos = new List<Producto>(); 
                TempData["Error"] = "Error al cargar la lista de productos: " + ex.Message;
            }

            // --- CAMBIO ---
            // Le decimos que busque la vista en la ruta específica que mencionaste.
            return View("~/Views/Home/AgregarProducto.cshtml");
        }

        [HttpPost]
        public IActionResult Guardar(int idProducto, string nombreEsp, DateTime fechaVencimiento, string foto)
        {

            // --- Verificación de Sesión (Comentada para testear) ---
            
            int? idHeladera = HttpContext.Session.GetInt32("IdHeladeraActual"); 
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario"); 

            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth"); //
            }
            if (idHeladera == null)
            {
                TempData["Error"] = "Tu sesión ha expirado o no has seleccionado una heladera.";
                return RedirectToAction("Seleccionar", "Heladera"); //
            }
            
            // --- Fin Verificación de Sesión ---

            try
            {
                // La llamada al método de 5 argumentos
                BD.agregarProductoExistente(
                    idHeladera.Value,
                    idProducto,
                    nombreEsp,
                    fechaVencimiento,
                    foto
                );

                TempData["Success"] = "¡Producto agregado con éxito! (Modo Test)";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo agregar el producto: " + ex.Message;
                ViewBag.ListaProductos = BD.GetAllProductos(); //
                
                // --- CAMBIO ---
                // Si hay un error, debe volver a la misma vista.
                return View("~/Views/Home/AgregarProducto.cshtml");
            }

            return RedirectToAction("MiHeladera", "Home"); //
        }
    }
}