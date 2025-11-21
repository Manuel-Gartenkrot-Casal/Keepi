using KeepiProg.Models; 
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace KeepiProg.Controllers
{
    public class RecetasController : Controller
    {
        
        private int? GetCurrentUserId()


        
        {
          return HttpContext.Session.GetInt32("IdUsuario");
        }

        public IActionResult Index()
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.ListaRecetas = BD.GetAllRecetas();
            

            return View("~/Views/Home/Recetas.cshtml");
        }

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


            return View("~/Views/Home/Detalles.cshtml", receta);
        }


        [HttpPost]
        public IActionResult FinalizarReceta(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            
            BD.IncrementarPopularidadReceta(id);
            
            return RedirectToAction("Detalles", new { id = id });
        }   
[HttpPost]
        public IActionResult ToggleFavorito(int id)
        {
            if (GetCurrentUserId() == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            BD.ToggleFavoritoReceta(id);

            return RedirectToAction("Detalles", new { id = id });
        }
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

            return View("~/Views/Home/SugerirRecetas.cshtml", recetasSugeridas);
        }
    }
}
