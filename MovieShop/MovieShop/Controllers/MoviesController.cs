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
        public async Task<IActionResult> Genre(int id, int page = 1, int pageSize = 30)
        {

            var pagedGenreSet = await _movieService.GetMoviesByGenre(id, page, pageSize);
            return View(pagedGenreSet);
        }
    }
}