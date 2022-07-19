using Microsoft.AspNetCore.Mvc;
using MovieShop.Models;
using System.Diagnostics;

namespace MovieShop.Controllers
{
    public class CastController : Controller
    {
        private readonly ILogger<CastController> _logger;

        public CastController(ILogger<CastController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}