using Microsoft.AspNetCore.Mvc;
using MovieShop.Models;
using System.Diagnostics;
using ApplicationCore.Contracts.Services;

namespace MovieShop.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _movieService.GetMovieDetails(id);
            return View(movieDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Genre(int id, string name, int page)
        {
            ViewData["genreId"] = id;
            ViewData["genreName"] = name;
            ViewData["pageNo"] = page;
            // total number of pages
            // total count of movies / 30 to get toital number pages
            var movieCards = await _movieService.GetMoviesByGenre(id, page);
            return View(movieCards);
        }
    }
}