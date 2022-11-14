using System.Security.Claims;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.WebApi.Adapters;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Builders;

public class RecipeBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRatingRepository _ratingRepository;
    private readonly IUserRepository _userRepository;

    public RecipeBuilder(IUserRepository userRepository,
        IRatingRepository ratingRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _ratingRepository = ratingRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DetailRecipeDto> BuildRecipeDetail(Recipe recipe)
    {
        var rating = await GetRating(ClaimsEmail(), recipe.RecipeId);
        var author = await _userRepository.GetById(recipe.UserId);
        return recipe.ToDetailDto(rating, author?.Name);
    }

    public async Task<List<RecipeDto>> BuildRecipes(IReadOnlyList<Recipe> recipes)
    {
        var authorIds = recipes.Select(x => x.UserId).Distinct().ToList();
        var authors = await _userRepository.GetByIds(authorIds);
        var authorByUserIdDictionary = authors.ToDictionary(x => x.UserId);

        Dictionary<int, Rating> ratingByRecipeId = new();
        Rating? rating;
        var claimsEmail = ClaimsEmail();
        if (claimsEmail is not null)
        {
            var recipeIds = recipes.Select(x => x.RecipeId).Distinct().ToList();
            var user = await _userRepository.GetByEmail(claimsEmail);
            if (user is null)
                throw new Exception("User not found.");
            var ratings = await _ratingRepository.Get(user.UserId, recipeIds);
            ratingByRecipeId = ratings.ToDictionary(x => x.RecipeId);
        }

        return recipes.Select(x =>
        {
            rating = ratingByRecipeId.GetValueOrDefault(x.RecipeId);
            var author = authorByUserIdDictionary.GetValueOrDefault(x.UserId);
            return x.ToDto(rating, author?.Name);
        }).ToList();
    }

    private async Task<Rating?> GetRating(string? email, int recipeId)
    {
        if (email is null) return null;

        var user = await _userRepository.GetByEmail(email);
        Rating? rating = null;
        if (user is not null)
            rating = await _ratingRepository.Get(user.UserId, recipeId);

        return rating;
    }

    private string? ClaimsEmail()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    }
}