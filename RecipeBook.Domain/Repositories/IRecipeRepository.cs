using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories;

public interface IRecipeRepository
{
    Task Add(Recipe recipe);
    Task Delete(int id);
    Task Edit(Recipe existingRecipe, Recipe editedRecipe);
    Task<Recipe?> GetById(int id);
    Task<Recipe?> GetRecipeOfDay();
    Task<IReadOnlyList<Recipe>> GetInUserOwnedByUserId(int userId);
    Task<int> GetUserRecipesCountByUserId(int userId);
    Task<IReadOnlyList<Recipe>> Search(int skip, int take, IEnumerable<int> recipeIds);
    Task<IReadOnlyList<Recipe>> Search(int skip, int take, string searchQuery);
}