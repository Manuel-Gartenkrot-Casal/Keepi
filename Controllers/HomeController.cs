using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KeepiProg.Models;

namespace KeepiProg.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
        public IActionResult Perfil(){

        return View();

        }

}
