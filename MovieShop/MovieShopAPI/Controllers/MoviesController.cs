using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchMovies(string title, int page = 1, int pageSize = 30)
        {
            var pagedSearchSet = await _movieService.GetMoviesBySearch(title, page, pageSize);
            if (pagedSearchSet == null || pagedSearchSet.TotalRowCount == 0)
            {
                return NotFound(new { errorMessage = "No movies found in that genre" });
            }
            return Ok(pagedSearchSet);

        }

        [HttpGet]
        [Route("top-grossing")]
        public async Task<IActionResult> GetTopGrossingMovies()
        {
            var movies = await _movieService.GetTopRevenueMovies();
            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "No Movies Found" });
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { errorMessage = $"No movie found for {id}" });
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("top-rated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMovies();
            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "No Movies Found" });
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("Genre/{genreId:int}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId, int page = 1, int pageSize = 30)
        {
            var pagedGenreSet = await _movieService.GetMoviesByGenre(genreId, page, pageSize);
            if (pagedGenreSet == null || pagedGenreSet.TotalRowCount == 0)
            {
                return NotFound(new { errorMessage = "No movies found in that genre" });
            }
            return Ok(pagedGenreSet);
        }

        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetMovieReviews(int id)
        {
            var pagedReviewSet = await _movieService.GetReviewsByMovie(id);
            if (pagedReviewSet == null || pagedReviewSet.TotalRowCount == 0)
            {
                return NotFound(new { errorMessage = "No reviews found for that movie" });
            }
            return Ok(pagedReviewSet);
        }
    }
}
