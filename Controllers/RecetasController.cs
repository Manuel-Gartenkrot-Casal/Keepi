using KeepiProg.Models; // Asegúrate que el namespace sea el correcto
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq; // Necesario para .Select() y .All()

namespace KeepiProg.Controllers
{
    public class RecetasController : Controller
    {
        
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

        /// <summary>
        /// Muestra el listado de TODAS las recetas.
        /// Usa la vista: /Views/Home/Recetas.cshtml
        /// </summary>
        public IActionResult Index()
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.ListaRecetas = BD.GetAllRecetas();
            
            // ¡IMPORTANTE! Renderizamos la vista específica de la carpeta Home
            return View("~/Views/Home/Recetas.cshtml");
        }

        /// <summary>
        /// Muestra los detalles de UNA receta.
        /// Usa la vista: /Views/Home/Detalles.cshtml
        /// </summary>
        public IActionResult Detalles(int id)
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
            ViewBag.ProductosDeLaReceta = BD.GetProductosByRecetaId(id);

            // ¡IMPORTANTE! Renderizamos la vista específica de la carpeta Home
            return View("~/Views/Home/Detalles.cshtml", receta);
        }

        /// <summary>
        /// Acción para sumar +1 a la popularidad.
        /// </summary>
        [HttpPost]
        public IActionResult FinalizarReceta(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            
            BD.IncrementarPopularidadReceta(id);
            
            // Redirige de vuelta a la vista de Detalles de esa receta
            return RedirectToAction("Detalles", new { id = id });
        }

        /// <summary>
        /// Muestra solo las recetas que el usuario puede hacer.
        /// Usa la vista: /Views/Home/SugerirRecetas.cshtml
        /// </summary>
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
                // ¡IMPORTANTE! Renderizamos la vista específica de la carpeta Home
                return View("~/Views/Home/SugerirRecetas.cshtml", new List<Receta>()); 
            }

            List<Producto> productosEnHeladera = BD.GetProductosByHeladeraId(heladera.ID); 
            HashSet<int> idsProductosEnHeladera = new HashSet<int>(productosEnHeladera.Select(p => p.ID));

            List<Receta> todasLasRecetas = BD.GetAllRecetas();
            List<Receta> recetasSugeridas = new List<Receta>();

            foreach (var receta in todasLasRecetas)
            {
                List<Producto> productosRequeridos = BD.GetProductosByRecetaId(receta.ID); 
                bool sePuedeHacer = productosRequeridos.Count > 0 && productosRequeridos.All(pr => idsProductosEnHeladera.Contains(pr.ID));
                
                if (sePuedeHacer)
                {
                    recetasSugeridas.Add(receta);
                }
            }
            
            // ¡IMPORTANTE! Renderizamos la vista específica de la carpeta Home
            return View("~/Views/Home/SugerirRecetas.cshtml", recetasSugeridas);
        }
    }
}