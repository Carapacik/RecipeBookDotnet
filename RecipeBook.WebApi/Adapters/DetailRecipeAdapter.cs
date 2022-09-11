using RecipeBook.Domain.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class DetailRecipeAdapter
{
    public static DetailRecipeDto ToDetailDto(this Recipe recipe, Rating? rating, string? authorName)
    {
        return new DetailRecipeDto
        {
            RecipeId = recipe.RecipeId,
            Title = recipe.Title,
            Description = recipe.Description,
            ImageUrl = recipe.ImageUrl,
            CookingTimeInMinutes = recipe.CookingTimeInMinutes,
            PortionsCount = recipe.PortionsCount,
            LikesCount = recipe.LikesCount,
            FavoritesCount = recipe.FavoritesCount,
            AuthorName = authorName,
            IsLiked = rating?.IsLiked ?? false,
            IsFavorite = rating?.InFavorite ?? false,
            Tags = recipe.Tags.Select(x => x.Name).ToList(),
            Steps = recipe.Steps.OrderBy(x => x.Number).Select(x => x.Description).ToList(),
            Ingredients = recipe.Ingredients.Select(x => new IngredientDto
            {
                Title = x.Title, IngredientNames = x.IngredientItems.Select(y => y.Name).ToList()
            }).ToList()
        };
    }
}