using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Models;
using System.Diagnostics;

namespace MovieShop.Controllers
{
    public class CastController : Controller
    {
        private readonly ILogger<CastController> _logger;
        private readonly ICastService _castService;

        public CastController(ILogger<CastController> logger, ICastService castService)
        {
            _logger = logger;
            _castService = castService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var castDetails = await _castService.GetCastDetails(Id);
            return View(castDetails);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}