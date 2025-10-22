using KeepiProg.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
// No se usa System.Linq, como pediste

namespace KeepiProg.Controllers
{
    public class RecetasController : Controller
    {
        // --- MÉTODO DE AYUDA PARA AUTENTICACIÓN ---
        
        private int? GetCurrentUserId()
        {
            string idUsuarioStr = HttpContext.Session.GetString("IdUsuario");
            if (string.IsNullOrEmpty(idUsuarioStr))
            {
                return null;
            }
            if (int.TryParse(idUsuarioStr, out int idUsuario))
            {
                return idUsuario;
            }
            return null;
        }

        // --- ACCIONES CRUD (Create, Read, Update, Delete) ---

        public IActionResult Index()
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.ListaRecetas = BD.GetAllRecetas();
            return View();
        }

        public IActionResult Detalles(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            
            // Usamos el 'id' que llega como parámetro
            Receta receta = BD.GetRecetaById(id); 
            if (receta == null)
            {
                return NotFound();
            }
            ViewBag.ProductosDeLaReceta = BD.GetProductosByRecetaId(id);
            return View(receta);
        }

        public IActionResult Crear()
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Receta receta)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {
                BD.CrearReceta(receta);
                return RedirectToAction("Index");
            }
            return View(receta);
        }

        public IActionResult Editar(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            Receta receta = BD.GetRecetaById(id);
            if (receta == null)
            {
                return NotFound();
            }
            ViewBag.TodosLosProductos = BD.GetAllProductos();
            ViewBag.ProductosEnReceta = BD.GetProductosByRecetaId(id);
            return View(receta);
        }

        [HttpPost]
        public IActionResult Editar(Receta receta)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {
                BD.UpdateReceta(receta);
                return RedirectToAction("Index");
            }
            return View(receta);
        }

        public IActionResult Eliminar(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            Receta receta = BD.GetRecetaById(id);
            if (receta == null)
            {
                return NotFound();
            }
            return View(receta);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            BD.DeleteProductosXRecetaByIdReceta(id);
            BD.DeleteReceta(id);
            return RedirectToAction("Index");
        }

        // --- ACCIÓN PERSONALIZADA: SUGERIR RECETAS (Corregida) ---

        public IActionResult SugerirRecetas()
        {
            int? idUsuario = GetCurrentUserId();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            Heladera heladera = BD.GetHeladeraByUsuarioId(idUsuario.Value); 
            if (heladera == null)
            {
                ViewBag.Error = "No tienes ninguna heladera asignada.";
                return View(new List<Receta>()); 
            }

            // CORRECCIÓN: heladera.ID en lugar de heladera.IdHeladera
            List<Producto> productosEnHeladera = BD.GetProductosByHeladeraId(heladera.ID); 
            
            HashSet<int> idsProductosEnHeladera = new HashSet<int>();
            foreach (Producto prod in productosEnHeladera)
            {
                // CORRECCIÓN: prod.ID en lugar de prod.IdProducto
                idsProductosEnHeladera.Add(prod.ID); 
            }

            List<Receta> todasLasRecetas = BD.GetAllRecetas();
            List<Receta> recetasSugeridas = new List<Receta>();

            foreach (var receta in todasLasRecetas)
            {
                // CORRECCIÓN: receta.ID en lugar de receta.IdReceta
                List<Producto> productosRequeridos = BD.GetProductosByRecetaId(receta.ID); 
                
                bool sePuedeHacer = true; 
                
                if (productosRequeridos.Count == 0)
                {
                    sePuedeHacer = false; 
                }
                else
                {
                    foreach (var prodRequerido in productosRequeridos)
                    {
                        // CORRECCIÓN: prodRequerido.ID en lugar de prodRequerido.IdProducto
                        if (!idsProductosEnHeladera.Contains(prodRequerido.ID)) 
                        {
                            sePuedeHacer = false;
                            break;
                        }
                    }
                }
                if (sePuedeHacer)
                {
                    recetasSugeridas.Add(receta);
                }
            }
            
            return View(recetasSugeridas);
        }

        // --- Acciones para AJAX (Opcional) ---

        [HttpPost]
        public IActionResult AgregarProductoAReceta(int idReceta, int idProducto)
        {
             if (GetCurrentUserId() == null)
            {
                return Json(new { success = false, message = "No autorizado" });
            }
            BD.AgregarProductoAReceta(idReceta, idProducto);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult QuitarProductoDeReceta(int idReceta, int idProducto)
        {
             if (GetCurrentUserId() == null)
            {
                return Json(new { success = false, message = "No autorizado" });
            }
            BD.QuitarProductoDeReceta(idReceta, idProducto);
            return Json(new { success = true });
        }
    }
}