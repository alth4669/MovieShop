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
        Task<GenrePageModel<Movie>> GetByGenre(int genreId, int page, int pageSize);
        Task<Movie> GetById(int id);
    }
}