namespace RecipeBook.Application.Services;

public interface IRatingService
{
    Task AddToFavorites(int recipeId);
    Task AddToLikes(int recipeId);
    Task RemoveFromFavorites(int recipeId);
    Task RemoveFromLikes(int recipeId);
}