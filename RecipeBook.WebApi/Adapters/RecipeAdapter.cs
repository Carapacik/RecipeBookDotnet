using RecipeBook.Domain.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class RecipeAdapter
{
    public static RecipeDto ToDto(this Recipe recipe, Rating? rating, string? authorName)
    {
        return new RecipeDto
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
            Tags = recipe.Tags.Select(x => x.Name).ToList()
        };
    }
}