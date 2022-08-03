using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IMovieRepository
    {

        Task<List<Movie>> GetTop30HighestRevenueMovies();
        Task<List<Movie>> GetTop30RatedMovies();
        Task<GenrePageModel<Movie>> GetByGenre(int genreId, int page = 1, int pageSize = 30);
        Task<SearchPageModel<Movie>> GetByTitle(string title, int page = 1, int pageSize = 30);
        Task<Movie> GetById(int id);
        Task<decimal> GetMovieRating(int movieId);
        Task<ReviewPageModel<Review>> GetMovieReviews(int movieId, int page = 1, int pageSize = 30 );
    }
}