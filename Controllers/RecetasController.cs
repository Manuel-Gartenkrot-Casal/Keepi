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

public IActionResult Index(string busqueda, string filtro)
{
    int? idUsuario = GetCurrentUserId();
    if (idUsuario == null)
    {
        return RedirectToAction("Login", "Auth");
    }

    // 1. Obtener la lista base de recetas
    List<Receta> recetas;

    if (filtro == "Sugeridas")
    {
        // --- LÓGICA DE SUGERIR RECETAS INTEGRADA ---
        Heladera heladera = BD.GetHeladeraByUsuarioId(idUsuario.Value);
        
        if (heladera == null)
        {
            recetas = new List<Receta>(); // No tiene heladera, no hay sugerencias
        }
        else
        {
            List<Producto> productosEnHeladera = BD.GetProductosByHeladeraId(heladera.ID);
            HashSet<int> idsProductosEnHeladera = new HashSet<int>(productosEnHeladera.Select(p => p.ID));
            List<Receta> todas = BD.GetAllRecetas();
            recetas = new List<Receta>();

            foreach (var r in todas)
            {
                List<Producto> productosRequeridos = BD.GetProductosByRecetaId(r.ID);
                // Verifica que la receta tenga ingredientes y que el usuario tenga TODOS
                if (productosRequeridos.Count > 0 && productosRequeridos.All(pr => idsProductosEnHeladera.Contains(pr.ID)))
                {
                    recetas.Add(r);
                }
            }
        }
    }
    else
    {
        // Traer todas si no se pide sugerencia
        recetas = BD.GetAllRecetas();
    }

    // 2. Aplicar Buscador (si escribieron algo)
    if (!string.IsNullOrEmpty(busqueda))
    {
        recetas = recetas.Where(r => r.nombre.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    // 3. Aplicar Ordenamiento (Popularidad o Alfabético)
    switch (filtro)
    {
        case "Popularidad":
            recetas = recetas.OrderByDescending(r => r.Popularidad).ToList();
            break;
        case "Alfabeticamente":
            recetas = recetas.OrderBy(r => r.nombre).ToList();
            break;
        // Si es "Sugeridas", ya filtramos, pero podemos ordenarlas por nombre por defecto
        default: 
            if (filtro != "Sugeridas") 
                recetas = recetas.OrderBy(r => r.nombre).ToList(); 
            break;
    }

    // Guardamos los valores para mantenerlos en la vista
    ViewBag.CurrentFilter = filtro;
    ViewBag.CurrentSearch = busqueda;
    ViewBag.ListaRecetas = recetas;

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