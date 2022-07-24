using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;
        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }

        public async Task<CastDetailsModel> GetCastDetails(int castId)
        {
            var castDetails = await _castRepository.GetAllMovies(castId);
            var castDetailsModel = new CastDetailsModel
            {
                Id = castDetails.Id,
                Name = castDetails.Name,
                Gender = castDetails.Gender,
                TmdbUrl = castDetails.TmdbUrl,
                ProfilePath = castDetails.ProfilePath,
            };


            foreach (var movieCast in castDetails.MoviesOfCast)
            {
                castDetailsModel.Roles.Add(new MovieCastModel
                {
                    MovieId = movieCast.Movie.Id,
                    Character = movieCast.Character,
                    PosterUrl = movieCast.Movie.PosterUrl
                });
            }

            return castDetailsModel;
        }
    }
}
