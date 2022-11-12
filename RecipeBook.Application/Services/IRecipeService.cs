using RecipeBook.Application.Entities;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Application.Services;

public interface IRecipeService
{
    Task<int> AddRecipe(RecipeCommand recipeCommand);
    Task DeleteRecipe(int id);
    Task EditRecipe(RecipeCommand recipeCommand);
    Task<IReadOnlyList<Recipe>> GetFavoriteRecipes(int skip, int take);
    Task<IReadOnlyList<Recipe>> GetUserOwnedRecipes(int skip, int take);
    Task<DailyRecipeCommand?> GetRecipeOfDay();
    Task<IReadOnlyList<Recipe>> GetAllRecipes(int skip, int take, string? searchQuery);
    Task<Recipe?> GetDetailRecipe(int id);
}