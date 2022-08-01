using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        public UserService(IReportRepository reportRepository, IPurchaseRepository purchaseRepository)
        {
            _reportRepository = reportRepository;
            _purchaseRepository = purchaseRepository;
        }
        public async Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            var fav = await _reportRepository.GetFavoriteById(favoriteRequest.UserId, favoriteRequest.MovieId);
            if (fav != null)
            {
                throw new Exception("Favorite for this user and movie already exists!");
            }

            var dbFav = new Favorite
            {
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };
            var newFav = await _reportRepository.AddFavorite(dbFav);
            if (newFav.MovieId > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var review = await _reportRepository.GetReviewById(reviewRequest.UserId, reviewRequest.MovieId);
            if (review != null)
            {
                throw new Exception("Review for this user and movie already exists!");
            }

            var dbReview = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };
            var newReview = await _reportRepository.AddReview(dbReview);
            if (newReview.MovieId > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMovieReview(int userId, int movieId)
        {
            var review = await _reportRepository.DeleteReview(userId, movieId);
            if (review == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> FavoriteExists(int userId, int movieId)
        {
            var favorite = await _reportRepository.GetFavoriteById(userId, movieId);
            if (favorite == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<FavoriteRequestModel>> GetAllFavoritesForUser(int id)
        {
            var favorites = await _reportRepository.GetAllFavoriteForUser(id);
            var favModels = new List<FavoriteRequestModel>();
            foreach (var fav in favorites)
            {
                favModels.Add(new FavoriteRequestModel { MovieId = fav.MovieId, UserId = fav.UserId, PosterUrl = fav.Movie.PosterUrl });
            }
            return favModels;
        }

        public async Task<List<PurchaseRequestModel>> GetAllPurchasesForUser(int userId)
        {
            var purchases = await _purchaseRepository.GetAllById(userId);
            var purchaseModels = new List<PurchaseRequestModel>();
            foreach (var purchase in purchases)
            {
                purchaseModels.Add(new PurchaseRequestModel
                {
                    PurchaseNumber = purchase.PurchaseNumber.ToString(),
                    TotalPrice = purchase.TotalPrice,
                    PurchaseDateTime = purchase.PurchaseDateTime,
                    MovieId = purchase.MovieId,
                    PosterUrl = purchase.Movie.PosterUrl
                });
            }
            return purchaseModels;
        }

        public async Task<List<ReviewRequestModel>> GetAllReviewsByUser(int userId)
        {
            var reviews = await _reportRepository.GetUserReviews(userId);
            var reviewModels = new List<ReviewRequestModel>();
            foreach (var review in reviews)
            {
                reviewModels.Add(new ReviewRequestModel
                {
                    MovieId = review.MovieId,
                    UserId = review.UserId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }
            return reviewModels;
        }

        public async Task<PurchaseRequestModel> GetPurchaseDetails(int userId, int movieId)
        {
            var purchase = await _purchaseRepository.GetByUserMovie(userId, movieId);
            return new PurchaseRequestModel
            {
                PurchaseNumber = purchase.PurchaseNumber.ToString(),
                TotalPrice = purchase.TotalPrice,
                PurchaseDateTime = purchase.PurchaseDateTime,
                MovieId = purchase.MovieId,
                PosterUrl = purchase.Movie.PosterUrl
            };
        }

        public async Task<ReviewRequestModel> GetReviewDetails(int userId, int movieId)
        {
            var review = await _reportRepository.GetReviewById(userId, movieId);
            if (review == null)
            {
                return null;
            }
            else
            {
                return new ReviewRequestModel
                {
                    MovieId = review.MovieId,
                    UserId = review.UserId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                };
            }
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            var purchase = await _purchaseRepository.GetByUserMovie(userId, purchaseRequest.MovieId);
            if (purchase == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            var purchase = await _purchaseRepository.GetByUserMovie(userId, purchaseRequest.MovieId);
            if (purchase != null)
            {
                throw new Exception("User has already purchased this movie!");
            }
            var dbPurchase = new Purchase
            {
                UserId = userId,
                PurchaseNumber = new Guid(purchaseRequest.PurchaseNumber),
                TotalPrice = (decimal) purchaseRequest.TotalPrice,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime,
                MovieId = purchaseRequest.MovieId
            };
            var newPurchase = await _purchaseRepository.AddPurchase(dbPurchase);
            if (newPurchase.UserId > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var review = await _reportRepository.DeleteFavorite(favoriteRequest.UserId, favoriteRequest.MovieId);
            if (review == null)
            {
                return false;
            }
            return true;
        }

        public async Task<ReviewRequestModel> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var updatedReview = await _reportRepository.UpdateReview(reviewRequest);
            if (updatedReview == null)
            {
                throw new Exception("No review for that user and movie could be found!");
            }
            return reviewRequest;
        }
    }
}
