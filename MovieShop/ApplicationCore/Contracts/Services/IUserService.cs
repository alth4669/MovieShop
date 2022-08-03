using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IUserService
    {
        public Task<UserModel> GetUserById(int userId);
        public Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId);
        public Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId);
        public Task<List<PurchaseRequestModel>> GetAllPurchasesForUser(int id);
        public Task<PurchaseRequestModel> GetPurchaseDetails(int userId, int movieId);
        public Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest);
        public Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest);
        public Task<bool> FavoriteExists(int id, int movieId);
        public Task<List<FavoriteRequestModel>> GetAllFavoritesForUser(int id);
        public Task<bool> AddMovieReview(ReviewRequestModel reviewRequest);
        public Task<ReviewRequestModel> UpdateMovieReview(ReviewRequestModel reviewRequest);
        public Task<bool> DeleteMovieReview(int userId, int movieId);
        public Task<List<ReviewRequestModel>> GetAllReviewsByUser(int id);
        public Task<ReviewRequestModel>? GetReviewDetails(int userId, int movieId);
    }
}
