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
            return movieDetails;
        }

        public async Task<List<Movie>> GetTop30HighestRevenueMovies()
        {
            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public Task<List<Movie>> GetTop30RatedMovies()
        {
            throw new NotImplementedException();
        }
    }
}
