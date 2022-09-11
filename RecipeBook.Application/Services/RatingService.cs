using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure;

namespace RecipeBook.Application.Services;

public class RatingService : IRatingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRatingRepository _ratingRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RatingService(IRecipeRepository recipeRepository,
        IRatingRepository ratingRepository,
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _recipeRepository = recipeRepository;
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task AddToFavorites(int recipeId)
    {
        var recipe = await _recipeRepository.GetById(recipeId);
        if (recipe == null) throw new Exception($"Recipe with id [{recipeId}] does not exist");

        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null) throw new Exception("User not found.");

        var rating = await _ratingRepository.Get(user.UserId, recipeId);
        if (rating == null)
        {
            rating = new Rating
            {
                UserId = user.UserId,
                RecipeId = recipeId
            };
            await _ratingRepository.Add(rating);
        }

        if (rating.InFavorite) return;

        rating.InFavorite = true;
        rating.ModificationDateTime = DateTime.Now;
        recipe.FavoritesCount += 1;

        if (rating.IsLiked) return;

        rating.IsLiked = true;
        recipe.LikesCount += 1;
        await _unitOfWork.Commit();
    }

    public async Task RemoveFromFavorites(int recipeId)
    {
        var recipe = await _recipeRepository.GetById(recipeId);
        if (recipe == null) throw new ArgumentException($"Recipe with id:{recipeId} does not exist");

        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null) throw new Exception("User not found.");

        var rating = await _ratingRepository.Get(user.UserId, recipeId);
        if (rating is not {InFavorite: true}) return;

        rating.InFavorite = false;
        rating.ModificationDateTime = DateTime.Now;
        recipe.FavoritesCount -= 1;
        await _unitOfWork.Commit();
    }

    public async Task AddToLikes(int recipeId)
    {
        var recipe = await _recipeRepository.GetById(recipeId);
        if (recipe == null) throw new ArgumentException($"Recipe with id [{recipeId}] does not exist");

        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null) throw new Exception("User not found.");
        var rating = await _ratingRepository.Get(user.UserId, recipeId);
        if (rating == null)
        {
            rating = new Rating
            {
                UserId = user.UserId,
                RecipeId = recipeId
            };
            await _ratingRepository.Add(rating);
        }

        if (rating.IsLiked) return;

        rating.IsLiked = true;
        recipe.LikesCount += 1;
        await _unitOfWork.Commit();
    }

    public async Task RemoveFromLikes(int recipeId)
    {
        var recipe = await _recipeRepository.GetById(recipeId);
        if (recipe == null) throw new ArgumentException($"Recipe with id [{recipeId}] does not exist");

        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null) throw new Exception("User not found.");
        var rating = await _ratingRepository.Get(user.UserId, recipeId);
        if (rating is not {IsLiked: true}) return;

        rating.IsLiked = false;
        recipe.LikesCount -= 1;
        await _unitOfWork.Commit();
    }

    private string ClaimsEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        if (email == null) throw new Exception("Invalid user.");
        return email;
    }
}