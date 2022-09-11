using RecipeBook.Application.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class DailyRecipeAdapter
{
    public static DailyRecipeDto ToDto(this DailyRecipeCommand recipe)
    {
        return new DailyRecipeDto
        {
            RecipeId = recipe.RecipeId,
            Title = recipe.Title,
            Description = recipe.Description,
            ImageUrl = recipe.ImageUrl,
            LikesCount = recipe.LikesCount,
            CookingTimeInMinutes = recipe.CookingTimeInMinutes,
            Author = recipe.Author
        };
    }
}