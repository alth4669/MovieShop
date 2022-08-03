using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IMovieService
    {
        Task<List<MovieCardModel>> GetTopRevenueMovies();
        Task<List<MovieCardModel>> GetTopRatedMovies();
        Task<MovieDetailsModel> GetMovieDetails(int movieId);
        Task<GenrePageModel<MovieCardModel>> GetMoviesByGenre(int genreId, int page = 1, int pageSize = 30);
        Task<SearchPageModel<MovieCardModel>> GetMoviesBySearch(string title, int page = 1, int pageSize = 30);
        Task<ReviewPageModel<ReviewModel>> GetReviewsByMovie(int movieId, int page = 1, int pageSize = 30); 
        
    }
}