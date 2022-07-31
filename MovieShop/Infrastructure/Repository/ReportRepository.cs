using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly MovieShopDbContext _movieShopDbContext;
        public ReportRepository(MovieShopDbContext dbContext)
        {
            _movieShopDbContext = dbContext;
        }
        public async Task<Favorite> AddFavorite(Favorite favorite)
        {
            _movieShopDbContext.Favorites.Add(favorite);
            await _movieShopDbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<Review> AddReview(Review review)
        {
            _movieShopDbContext.Reviews.Add(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review> DeleteReview(int userId, int movieId)
        {
            var review = await GetReviewById(userId, movieId);
            _movieShopDbContext.Reviews.Remove(review);
            await _movieShopDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Favorite> GetFavoriteById(int userId, int movieId)
        {
            var fav = await _movieShopDbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);
            return fav;
        }

        public async Task<List<Favorite>> GetAllFavoriteForUser(int userId)
        {
            var favs = await _movieShopDbContext.Favorites.Where(f => f.UserId == userId)
            .Include(f => f.Movie)
            .ToListAsync();
            return favs;
        }

        public async Task<List<Review>> GetUserReviews(int userId)
        {
            var reviews = await _movieShopDbContext.Reviews.Where(r => r.UserId == userId).ToListAsync();
            return reviews;
        }

        public async Task<Favorite> DeleteFavorite(int userId, int movieId)
        {
            var favorite = await GetFavoriteById(userId, movieId);
            _movieShopDbContext.Favorites.Remove(favorite);
            await _movieShopDbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<Review> GetReviewById(int userId, int movieId)
        {
            var review = await _movieShopDbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
            return review;
        }

        public async Task<Review> UpdateReview(ReviewRequestModel updatedReview)
        {
            var targetReview = await GetReviewById(updatedReview.UserId, updatedReview.MovieId);
            targetReview.Rating = updatedReview.Rating;
            targetReview.ReviewText = updatedReview.ReviewText;
            await _movieShopDbContext.SaveChangesAsync();
            return targetReview;
        }
    }
}
