using ApplicationCore.Entities;
using ApplicationCore.Contracts.Repository;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;
using System.Text.RegularExpressions;

namespace Infrastructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public MovieRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }

        public async Task<GenrePageModel<Movie>> GetByGenre(int genreId, int page = 1, int pageSize = 30)
        {
            var genreCount = await _movieShopDbContext.MovieGenres.Where(g => g.GenreId == genreId).CountAsync();
            var genreName = await _movieShopDbContext.Genres.Where(g => g.Id == genreId).Select(g => g.Name).FirstOrDefaultAsync();
            if (genreCount == 0)
            {
                throw new Exception("No movies found in that genre!");
            }

            var movies = await _movieShopDbContext.Movies
                .Include(m => m.GenresOfMovie)
                .Where(m => m.GenresOfMovie.Any(g => g.GenreId == genreId)).Skip((page-1)*pageSize).Take(pageSize)
                .ToListAsync();

            return new GenrePageModel<Movie>(genreName, movies, page, pageSize, genreCount);
        }

        public async Task<Movie> GetById(int id)
        {
            var movieDetails = await _movieShopDbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(m => m.Genre)
                .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .FirstOrDefaultAsync(m => m.Id == id);

            //var rating = await _movieShopDbContext.Reviews.Where(r => r.MovieId == id).DefaultIfEmpty().AverageAsync(r => r == null ? 0 : r.Rating);

            //movieDetails.Rating = rating;

            movieDetails.Rating = await GetMovieRating(id);

            return movieDetails;
        }

        public async Task<SearchPageModel<Movie>> GetByTitle(string title, int page = 1, int pageSize = 30)
        {
            var pattern = @"\b.*" + title + @".*\b";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            /*var hits = await _movieShopDbContext.Movies.Where(m => regex.IsMatch(m.Title)).ToListAsync();*/
            var hits = await _movieShopDbContext.Movies.Where(m => m.Title.Contains(title)).CountAsync();
            if (hits == 0)
            {
                throw new Exception("No movies found matching that title");
            }

            /*var movies = await _movieShopDbContext.Movies.Where(m => regex.IsMatch(m.Title)).Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();*/
            var movies = await _movieShopDbContext.Movies.Where(m => m.Title.Contains(title)).Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return new SearchPageModel<Movie>(title, movies, page, pageSize, hits);
        }

        public async Task<decimal> GetMovieRating(int movieId)
        {
            return await _movieShopDbContext.Reviews.Where(r => r.MovieId == movieId).DefaultIfEmpty().AverageAsync(r => r == null ? 0 : r.Rating);
        }

        public async Task<ReviewPageModel<Review>> GetMovieReviews(int movieId, int page = 1, int pageSize = 1)
        {
            var reviews = await _movieShopDbContext.Reviews.Include(r => r.Movie)
            .Where(r => r.MovieId == movieId)
            .OrderByDescending(r => r.CreatedDate)
            .Skip((page-1) * pageSize).Take(pageSize)
            .ToListAsync();
            return new ReviewPageModel<Review>(reviews.First().Movie.Title, reviews, page, pageSize, reviews.Count());
        }

        public async Task<List<Movie>> GetTop30HighestRevenueMovies()
        {
            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public async Task<List<Movie>> GetTop30RatedMovies()
        {
            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => m.Rating)
            .Take(30)
            .ToListAsync();
            return movies;
        }
    }
}
