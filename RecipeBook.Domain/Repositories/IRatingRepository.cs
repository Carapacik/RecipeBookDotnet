using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories;

public interface IRatingRepository
{
    Task Add(Rating rating);
    Task<Rating?> Get(int userId, int recipeId);
    Task<IReadOnlyList<Rating>> Get(int userId, IEnumerable<int> recipeIds);
    Task<IReadOnlyList<Rating>> GetInFavoriteByUserId(int userId);
    Task<int> GetUserLikesCountByUserId(int userId);
    Task<int> GetUserFavoritesCountByUserId(int userId);
}