using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeBookDbContext _context;

    public RecipeRepository(RecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task Add(Recipe recipe)
    {
        await _context.Recipes!.AddAsync(recipe);
    }

    public async Task Delete(int id)
    {
        var recipe = await GetById(id);
        if (recipe == null) throw new Exception("User not found.");
        _context.Recipes!.Remove(recipe);
    }

    public Task Edit(Recipe existingRecipe, Recipe editedRecipe)
    {
        if (!string.IsNullOrWhiteSpace(editedRecipe.ImageUrl)) existingRecipe.ImageUrl = editedRecipe.ImageUrl;

        existingRecipe.Title = editedRecipe.Title;
        existingRecipe.Description = editedRecipe.Description;
        existingRecipe.CookingTimeInMinutes = editedRecipe.CookingTimeInMinutes;
        existingRecipe.PortionsCount = editedRecipe.PortionsCount;
        existingRecipe.Tags = editedRecipe.Tags;
        existingRecipe.Steps = editedRecipe.Steps;
        existingRecipe.Ingredients = editedRecipe.Ingredients;
        return Task.CompletedTask;
    }

    public async Task<Recipe?> GetById(int id)
    {
        return await GetQuery().FirstOrDefaultAsync(x => x.RecipeId == id);
    }

    public async Task<Recipe?> GetRecipeOfDay()
    {
        var window = DateTime.Now.AddDays(-1);
        var recipeWithMoreLikesInLastDay = await GetQuery()
            .Where(x => x.CreationDateTime > window)
            .OrderByDescending(x => x.LikesCount)
            .FirstOrDefaultAsync();
        if (recipeWithMoreLikesInLastDay != null) return recipeWithMoreLikesInLastDay;

        var recipeWithMostLikes = await GetQuery().OrderByDescending(x => x.LikesCount)
            .FirstOrDefaultAsync();

        return recipeWithMostLikes;
    }

    public async Task<IReadOnlyList<Recipe>> GetInUserOwnedByUserId(int userId)
    {
        return await _context.Recipes!
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreationDateTime)
            .ToListAsync();
    }

    public async Task<int> GetUserRecipesCountByUserId(int userId)
    {
        return await _context.Recipes!.CountAsync(x => x.UserId == userId);
    }

    public async Task<IReadOnlyList<Recipe>> Search(int skip, int take, IEnumerable<int> recipeIds)
    {
        return await GetQuery().Where(x => recipeIds.Distinct().Contains(x.RecipeId))
            .OrderByDescending(x => x.CreationDateTime)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Recipe>> Search(int skip, int take, string searchQuery)
    {
        var query = GetQuery();
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var trimmedQuery = searchQuery.ToLower().Trim();
            query = query.Where(x =>
                x.Title.ToLower().Contains(trimmedQuery)
                || x.Tags.Any(y => y.Name.ToLower().Contains(trimmedQuery)));
        }

        return await query.OrderByDescending(x => x.CreationDateTime)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    private IQueryable<Recipe> GetQuery()
    {
        return _context.Recipes!
            .Include(x => x.Tags)
            .Include(x => x.Steps)
            .Include(x => x.Ingredients)
            .ThenInclude(y => y.IngredientItems);
    }
}