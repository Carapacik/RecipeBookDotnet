using RecipeBook.Application.Entities;
using RecipeBook.Domain.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class CreateRecipeAdapter
{
    public static async Task<RecipeCommand> FromDtoAsync(this CreateRecipeDto recipe, string email)
    {
        var fileAdapter = await FormFileAdapter.Create(recipe.RecipeImage);
        return new RecipeCommand(recipe.RecipeId,
            recipe.Title,
            recipe.Description,
            recipe.CookingTimeInMinutes,
            recipe.PortionsCount,
            email,
            recipe.Tags,
            recipe.Steps,
            recipe.Ingredients.Select(x => new Ingredient
            {
                Title = x.Title, IngredientItems = x.IngredientNames.Select(y => new IngredientItem {Name = y}).ToList()
            }).ToList(),
            fileAdapter.ConvertToStorageFile()
        );
    }

    private static FileResultCommand? ConvertToStorageFile(this FormFileAdapter? fileAdapter)
    {
        return fileAdapter is null
            ? null
            : new FileResultCommand(fileAdapter.Data, fileAdapter.FileExtension);
    }
}