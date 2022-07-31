using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IReportRepository
    {
        Task<Favorite> AddFavorite(Favorite favorite);
        Task<Favorite> DeleteFavorite(int userId, int movieId);
        Task<Favorite> GetFavoriteById(int userId, int movieId);
        Task<List<Favorite>> GetAllFavoriteForUser(int userId);
        Task<Review> AddReview(Review review);
        Task<Review> GetReviewById(int userId, int movieId);
        Task<Review> UpdateReview(ReviewRequestModel updatedReview);
        Task<Review> DeleteReview(int userId, int movieId);
        Task<List<Review>> GetUserReviews(int userId);
    }
}
