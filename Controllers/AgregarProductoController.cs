using Microsoft.AspNetCore.Mvc;
using Keepi.Models; 
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

             int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string? nombreHeladera = HttpContext.Session.GetString("nombreHeladera"); 
            
            if (string.IsNullOrEmpty(nombreHeladera))
            {
                TempData["Error"] = "Por favor, selecciona una heladera antes de agregar productos.";
                return RedirectToAction("Seleccionar", "Heladera"); 
            }
            
            Heladera heladeraActual = BD.SeleccionarHeladeraByNombre(idUsuario.Value, nombreHeladera);
            
            if (heladeraActual == null)
            {
                TempData["Error"] = "Error al cargar la heladera actual.";
                return RedirectToAction("Seleccionar", "Heladera"); 
            }

            try
            {
                List<Producto> productosBase = BD.GetAllProductos();
                ViewBag.ListaProductos = productosBase;
            }
            catch (Exception ex)
            {
                ViewBag.ListaProductos = new List<Producto>(); 
                TempData["Error"] = "Error al cargar la lista de productos: " + ex.Message;
            }

            return View("~/Views/Home/AgregarProducto.cshtml");
        }

        [HttpPost]
        public IActionResult Guardar(int idProducto, string nombreEsp, DateTime fechaVencimiento, string foto)
        {

            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario"); 

            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth"); 
            }
            
            string? nombreHeladera = HttpContext.Session.GetString("nombreHeladera"); 

            if (string.IsNullOrEmpty(nombreHeladera))
            {
                TempData["Error"] = "Tu sesión ha expirado o no has seleccionado una heladera.";
                return RedirectToAction("Seleccionar", "Heladera"); 
            }
            
            Heladera heladeraActual = BD.SeleccionarHeladeraByNombre(idUsuario.Value, nombreHeladera);
            
            if (heladeraActual == null)
            {
                TempData["Error"] = "Error al cargar la heladera actual o la heladera no existe.";
                return RedirectToAction("Seleccionar", "Heladera"); 
            }
            
            int idHeladera = heladeraActual.ID; 

            if (idProducto <= 0)
            {
                TempData["Error"] = "Por favor, selecciona un producto válido de la lista.";
                
                try 
                {
                    ViewBag.ListaProductos = BD.GetAllProductos(); 
                }
                catch (Exception dbEx)
                {
                    ViewBag.ListaProductos = new List<Producto>(); 
                    TempData["Error"] += " (Error al recargar productos: " + dbEx.Message + ")"; 
                }
                
                return View("~/Views/Home/AgregarProducto.cshtml");
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
                
                
                try 
                {
                    ViewBag.ListaProductos = BD.GetAllProductos(); 
                }
                catch (Exception dbEx)
                {
                    ViewBag.ListaProductos = new List<Producto>(); 
                    TempData["Error"] += " (Error al recargar productos: " + dbEx.Message + ")";
                }

                return RedirectToAction("CargarProductos", "Heladera");
            }

        return RedirectToAction("CargarProductos", "Heladera");
        }
    }
}