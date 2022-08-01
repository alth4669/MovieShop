using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Infra;
using MovieShop.Models;
using System.Diagnostics;

namespace MovieShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IHttpContextAccessor contextAccessor, ILogger<UserController> logger)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Review(ReviewRequestModel model)
        {
            ICurrentUser currentUser = new CurrentUser(_contextAccessor);
            if (currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            await _userService.AddMovieReview(model);
            return LocalRedirect("~/Movies/Details/" + model.MovieId);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseRequestModel model, int userId)
        {
            ICurrentUser currentUser = new CurrentUser(_contextAccessor);
            if (currentUser.IsAuthenticated == false)
            {
                return LocalRedirect("~/Account/Login");
            }
            await _userService.PurchaseMovie(model, userId);
            return LocalRedirect("~/Movies/Details/" + model.MovieId);
        }

        [HttpGet]
        public async Task<IActionResult> Purchases(int userId)
        {
            var purchases = await _userService.GetAllPurchasesForUser(userId);
            return View(purchases);
        }

        [HttpGet]
        public async Task<IActionResult> Favorites(int userId)
        {
            var favorites = await _userService.GetAllFavoritesForUser(userId);
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovieToFavorites(FavoriteRequestModel model)
        {
            if (await _userService.FavoriteExists(model.UserId, model.MovieId)) {
                throw new Exception("Favorite already exists!");
            }
            await _userService.AddFavorite(model);
            return LocalRedirect("~/Movies/Details/" + model.MovieId);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMovieFromFavorites(FavoriteRequestModel model)
        {
            if (await _userService.FavoriteExists(model.UserId, model.MovieId) == false)
            {
                throw new Exception("Cannot find a favorite to delete!");
            }
            await _userService.RemoveFavorite(model);
            return LocalRedirect("~/Movies/Details/" + model.MovieId);

        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseDetails(int userId, int movieId)
        {
            var details = await _userService.GetPurchaseDetails(userId, movieId);
            return PartialView("_PurchaseDetails", details);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReview(ReviewRequestModel model)
        {
            var updatedReview = await _userService.UpdateMovieReview(model);
            return LocalRedirect("~/Movies/Details/" + model.MovieId);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(ReviewRequestModel model)
        {
            await _userService.DeleteMovieReview(model.UserId, model.MovieId);
            return LocalRedirect("~/Movies/Details/" + model.MovieId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
